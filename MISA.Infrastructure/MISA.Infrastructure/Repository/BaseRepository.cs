using Dapper;
using MISA.Web02.Core.Interfaces;
using MISA.Web02.Core.MISAAttribute;
using MySqlConnector;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MISA.Web02.Infrastructor.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        #region DECLARE
        protected string _connectionString;
        protected string _tableName;
        #endregion

        #region CONSTRUCTOR
        public BaseRepository()
        {
            _connectionString = "Host=localhost;Port=5432;Database=misa_gd2_final;User Id=postgres;Password=1234";
            _tableName = ToSnakeCase(typeof(T).Name);
        }
        #endregion

        #region IBaseRepository METHODS
        public virtual IEnumerable<T> Get()
        {
            var sqlCommand = $"SELECT * FROM {_tableName}";
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                var result = new List<T>();

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dynamic entity = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var propName = ToPascalCase(reader.GetName(i));
                            var propValue = reader.GetValue(i);

                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }

                        T entityData = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity));

                        result.Add(entityData);
                    }
                }
                conn.Close();

                return result;
            }
        }

        public virtual T GetById(Guid entityId)
        {
            var columnIdName = _tableName + "_id";
            var sqlCommand = $"SELECT * FROM {_tableName} WHERE {columnIdName} = @m_entity_id ";

            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                T result = default;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@m_entity_id", entityId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dynamic entity = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);

                                ((IDictionary<string, object>)entity).Add(propName, propValue);
                            }

                            T entityData = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity));

                            result = entityData;
                        }
                    }
                }
                conn.Close();
                return result;
            }
        }
        public int Insert(T entity)
        {
            var columnList = new List<string>();
            var paramList = new List<string>();

            var props = typeof(T).GetProperties();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("", conn))
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        var prop = props[i];
                        var propName = ToSnakeCase(prop.Name);
                        var propValue = prop.GetValue(entity);

                        var isPrimaryKey = prop.IsDefined(typeof(PrimaryKey), true);
                        var isNotMap = prop.IsDefined(typeof(NotMappedProp), true);

                        if (!isPrimaryKey && !isNotMap && propValue != null)
                        {

                            columnList.Add(propName);
                            paramList.Add($"@{propName}");

                            cmd.Parameters.Add(new NpgsqlParameter($"@{propName}", propValue));
                        }
                    }
                    var columnString = string.Join(" ,", columnList);
                    var paramsString = string.Join(" ,", paramList);


                    var sqlCommandString = $"INSERT INTO {_tableName} ({columnString}) VALUES ({paramsString});";
                    cmd.CommandText = sqlCommandString;
                    var rowsAffected = cmd.ExecuteNonQuery();

                    conn.Close();

                    return rowsAffected;
                }
            }
        }

        public virtual int Update(Guid entityId, T entity)
        {
            var columnIdName = string.Empty;

            var props = typeof(T).GetProperties();
            string bodyString = "";
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("", conn))
                {

                    for (int i = 0; i < props.Length; i++)
                    {
                        var prop = props[i];
                        var propName = ToSnakeCase(prop.Name);
                        var propValue = prop.GetValue(entity);

                        var isPrimaryKey = prop.IsDefined(typeof(PrimaryKey), true);
                        var isNotMap = prop.IsDefined(typeof(NotMappedProp), true);
                        if (isPrimaryKey)
                        {
                            columnIdName = propName;
                        }

                        if (!isPrimaryKey && !isNotMap && propValue != null)
                        {
                            propValue = propValue == null ? DBNull.Value : propValue;
                            bodyString += $" {propName} = @{propName},";

                            cmd.Parameters.Add(new NpgsqlParameter($"@{propName}", propValue));
                        }

                    }
                    var sqlCommandString = $"UPDATE {_tableName} SET {bodyString.Substring(0, bodyString.Length - 1)} WHERE {columnIdName} = '{entityId}'";
                    cmd.CommandText = sqlCommandString;
                    var rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected;
                }


            }

        }
        public virtual int Delete(Guid entityId)
        {
            //1. Khai Báo dữ liệu truy vấn
            var idColumnName = $"{_tableName}_id";
            var sqlCommand =
                $"WITH deleted AS (DELETE FROM {_tableName} WHERE {idColumnName} = @m_entity_id IS TRUE RETURNING *) " +
                $"SELECT COUNT(*) FROM deleted";
            // Tạo kết nối tới csdl
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                int result = 0;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@m_entity_id", entityId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }
                    }
                }
                conn.Close();

                return result;
            }
        }
        public virtual bool CheckExistCode(string entityCode)
        {
            //1. Khai Báo dữ liệu truy vấn
            var entityCodeColumn = $"{_tableName}_code";

            var sqlCommand = $"SELECT {entityCodeColumn} FROM {_tableName} WHERE {entityCodeColumn} = @m_entity_code";

            // Tạo kết nối tới csdl
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                var result = string.Empty;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@m_entity_code", entityCode.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                }
                conn.Close();

                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }

                return true;
            }
        }

        public virtual bool CheckExistId(Guid entityId)
        {
            //1. Khai Báo dữ liệu truy vấn
            var entityIdColumn = $"{_tableName}_id";

            var sqlCommand = $"SELECT {entityIdColumn} FROM {_tableName} WHERE {entityIdColumn} = @m_entity_id";

            // Tạo kết nối tới csdl
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                var result = string.Empty;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue("@m_entity_id", entityId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                }
                conn.Close();

                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }

                return true;
            }

        }

        public virtual string GetNewCode()
        {
            // định nghĩa sql query dữ liệu 
            var sqlCommand = $"SELECT {_tableName}_code FROM {_tableName}";

            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn))
            {
                conn.Open();
                var newCode = string.Empty;

                using (var reader = cmd.ExecuteReader())
                {
                    // tạo danh sách code để lưu > tìm giá trị max trong danh sách     
                    var listCode = new List<int>();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            // chỉ lấy số mã code tìm được (VD: NCC00001 - 1)
                            var code = reader.GetString(i);
                            int extractedCode = int.Parse(Regex.Match(code, @"\d+").Value);
                            listCode.Add(extractedCode);
                        }
                    }
                    listCode.Sort();
                    // Lấy số max + 1 + chèn các số 0 vào đằng sau
                    newCode = $"{listCode.Max() + 1}".PadLeft(6, '0');

                }
                conn.Close();

                return newCode;
            }
        }

        public object FilterData(int pageIndex = 1, int pageSize = 10, string? filter = null)
        {
            // Định nghĩa tên function dùng trong DB
            var funcName = $"func_filter_{_tableName}";

            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(funcName, conn))
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                // Truyền tham số input
                cmd.Parameters.AddWithValue("m_page_index", pageIndex);
                cmd.Parameters.AddWithValue("m_page_size", pageSize);

                if (string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("m_filter", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("m_filter", filter);
                }

                var resultData = new List<object>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // tạo đối tượng dynamic để truyền key, value động vào
                        dynamic entity = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            // với mỗi cột đọc được > snake_case > PascalCase + lấy giá trị
                            var propName = ToPascalCase(reader.GetName(i));
                            var propValue = reader.GetValue(i);
                            var propType = reader.GetFieldType(i);

                            if (propType.IsPrimitive || propType.Name == "String")
                            {
                                entity = propValue;
                                continue;
                            }

                            // truyền vào key,value
                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }
                        // biến đổi entity về dạng object
                        object entityData = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(entity));

                        // lưu kết quả
                        resultData.Add(entityData);
                    }
                }
                // Đóng kết nối
                conn.Close();

                object result = JsonConvert.DeserializeObject<object>(resultData.FirstOrDefault().ToString());

                return result;
            }
        }
        public virtual int DeleteMultiple(List<Guid> entityIds)
        {
            //1. Khai Báo dữ liệu truy vấn
            var entityIdColumn = $"{_tableName}_id";

            // Biến chứa các employee ID
            var paramsListString = new List<string>();

            // Thêm id nhân viên vào lệnh sql
            for (int i = 0; i < entityIds.Count; i++)
            {
                paramsListString.Add($"(@m_entity_id_{i})");
            }
            // Thêm các OR ở giữa 
            var sqlColumnValue = string.Join(", ", paramsListString); ;

            // Query dữ liệu 
            var sqlCommand = $"DELETE FROM {_tableName} WHERE ({entityIdColumn}) IN ({sqlColumnValue});";

            // Chạy lệnh SQL
            // Tạo kết nối tới csdl postgres
            var result = 0;
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn))
            {
                conn.Open();

                for (int i = 0; i < entityIds.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@m_entity_id_{i}", entityIds[i].ToString());
                }
                result = cmd.ExecuteNonQuery();

                conn.Close();
            }

            return result;
        }
        #endregion

        #region HÀM TIỆN ÍCH
        /// <summary>
        /// Hàm biến đổi string từ snake_case > PascalCase
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ToPascalCase(string str)
        {
            string result = Regex.Replace(str, "_[a-z]", delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });

            result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }

        /// <summary>
        /// Hàm biến đổi string thường thành snake_case
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ToSnakeCase(string s)
        {
            string result = Regex.Replace(s, "[A-Z]", "_$0").ToLower();

            result = result[1..];

            return result;
        }


        #endregion

    }
}
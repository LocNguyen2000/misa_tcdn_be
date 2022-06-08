using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Web02.Infrastructor.Repositories;
using MISA.WEB02.Core.Entities;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    /// <summary>
    /// Lấy từ base repo dùng chung
    /// </summary>
    /// CREATED BY NHLOC - 20/04/22
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository() { }

        public List<Account> GetByParent(string accountCode)
        {
            // Định nghĩa tên function dùng trong DB
            var funcName = $"func_search_account_by_parent";
            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(funcName, conn))
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                // Truyền tham số input
                cmd.Parameters.AddWithValue("m_code", accountCode);

                var resultData = new List<Account>();

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

                            // truyền vào key,value
                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }
                        // biến đổi entity về dạng object
                        Account entityData = JsonConvert.DeserializeObject<Account>(JsonConvert.SerializeObject(entity));

                        // lưu kết quả
                        resultData.Add(entityData);
                    }
                }

                conn.Close();

                return resultData;
            }
        }
    }
}

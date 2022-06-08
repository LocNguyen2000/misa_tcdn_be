using MISA.Web02.Infrastructor.Repositories;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Interfaces;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Dynamic;

namespace MISA.WEB02.Infrastructure.Repository
{
    public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
    {
        #region DECLARE
        private IPaymentDetailRepository _paymentDetailRepository;
        #endregion

        #region CONSTRUCTOR
        public PaymentRepository(IPaymentDetailRepository paymentDetailRepository) {
            _paymentDetailRepository = paymentDetailRepository;
        }

        public object FilterAdvance(int pageIndex = 1, int pageSize = 10, string? filter = null, int? isRecord = null, DateTime? startDate = null, DateTime? endDate = null)
        {

            // Định nghĩa tên function dùng trong DB
            var funcName = $"func_filter_payment";

            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(funcName, conn))
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                // Truyền tham số input
                cmd.Parameters.AddWithValue("m_page_index", pageIndex);
                cmd.Parameters.AddWithValue("m_page_size", pageSize);

                // filter các cột
                if (string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("m_filter", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("m_filter", filter);
                }

                // filter theo trạng thái ghi sổ
                if (isRecord == null)
                {
                    cmd.Parameters.AddWithValue("m_is_record", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("m_is_record", isRecord.ToString());
                }

                // filter theo ngày bắt đầu, ngày kết thúc
                if (startDate == null)
                {
                    cmd.Parameters.AddWithValue("m_start_date", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("m_start_date", startDate?.ToString("dd/MM/yyyy"));
                }

                if (endDate == null)
                {
                    cmd.Parameters.AddWithValue("m_end_date", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("m_end_date", endDate?.ToString("dd/MM/yyyy"));
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
        #endregion

        #region METHODS
        /// <summary>
        /// Lấy dữ liệu chi tiêu + dữ liệu ncc
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Payment> Get()
        {
            // Truy vấn phiếu chi cùng với dữ liệu đối tượng nhà cung cấp
            var sqlCommand = "select  p.*,  v.vendor_code as account_object_code , v.vendor_name as account_object_name  " +
                "from payment p left join vendor v " +
                "on p.account_object_id = v.vendor_id;";
            
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                // Mở kết nối
                conn.Open();

                var result = new List<Payment>();

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Khởi tạo 1 đối tượng mới >> hứng các trường được đọc 
                        dynamic entity = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var propName = ToPascalCase(reader.GetName(i));
                            var propValue = reader.GetValue(i);

                            // Gán các trường vào đối tượng
                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }

                        // Cast về thành dữ liệu chi tiêu
                        Payment entityData = JsonConvert.DeserializeObject<Payment>(JsonConvert.SerializeObject(entity));
                        
                        // Thêm vào dữ liệu trả về
                        result.Add(entityData);
                    }
                }
                conn.Close();

                return result;
            }
        }

        
        public PaymentDetailGroup GetPaymentWithDetail(Guid paymentId)
        {
            // khởi tạo 1 đối tượng nhóm chi tiêu + hạch toán
            var paymentWithDetail = new PaymentDetailGroup();

            // lấy đối tượng chi tiêu
            paymentWithDetail.payment = base.GetById(paymentId);

            // lấy hạch toán theo id của chi tie
            paymentWithDetail.paymentDetail = _paymentDetailRepository.GetByPaymentId(paymentId);

            return paymentWithDetail;
        }

        

        #endregion
    }
}

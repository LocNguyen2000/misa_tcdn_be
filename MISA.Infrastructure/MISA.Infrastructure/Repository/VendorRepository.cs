using MISA.WEB02.Core.Entities;
using MISA.Web02.Infrastructor.Repositories;
using MISA.WEB02.Core.Interfaces;
using Npgsql;
using MISA.WEB02.Core.Resources;
using MISA.Core.Exceptions;

namespace MISA.WEB02.Infrastructure.Repository
{
    /// <summary>
    /// Repo dùng cho nhà cung cấp (Sử dụng PostgreSql)
    /// </summary>
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        #region DECLARE
        #endregion
        public VendorRepository() {}

        public override int DeleteMultiple(List<Guid> entityIds)
        {
            var errMsg = new Dictionary<string, string>();

            foreach (Guid id in entityIds)
            {
                var vendorCodeInPayment = GetVendorInPayment(id);
                if (!string.IsNullOrEmpty(vendorCodeInPayment))
                {
                    errMsg.Add("VendorCode", $"Nhà cung cấp <{vendorCodeInPayment}> {Resource.VN_ExistVendorInPayment}");
                    throw new MISAException(Resource.VN_MethodNotAllowed, errMsg);
                }
            }

            var rowsAffected = base.DeleteMultiple(entityIds);

            return rowsAffected;
        }

        public string GetVendorInPayment(Guid vendorId)
        {
            var sqlCommand = "select v.vendor_code " +
                "from payment p left join vendor v " +
                "on p.account_object_id = v.vendor_id " +
                "where p.account_object_id = @vendorId;";

            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                var result = string.Empty;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue("@vendorId", vendorId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = (reader.GetString(0));
                        }
                    }
                }
                conn.Close();

                return result;
            }
        }
    }
}

using MISA.Web02.Infrastructor.Repositories;
using MISA.WEB02.Core.Interfaces;
using MISA.Web02.Core.Entities;
using Npgsql;
using MISA.Web02.Core.MISAAttribute;
using System.Dynamic;
using Newtonsoft.Json;
using MISA.WEB02.Core.Resources;
using MISA.Core.Exceptions;

namespace MISA.WEB02.Infrastructure.Repository
{
    public class PaymentDetailRepository : BaseRepository<PaymentDetail>, IPaymentDetailRepository
    {
        #region CONSTRUCTOR
        public PaymentDetailRepository() { }
        #endregion

        #region METHODS

        public List<PaymentDetail> GetByPaymentId(Guid paymentId)
        {
            var result = new List<PaymentDetail>();

            var sqlCommand = $"SELECT pd.*, v.vendor_name as account_object_name, v.vendor_code as account_object_code " +
                $"FROM payment_detail pd left join vendor v " +
                $"on pd.account_object_id = v.vendor_id " +
                $"where pd.payment_id = @paymentId;";
            using (NpgsqlConnection? conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue("@paymentId", paymentId.ToString());
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

                            PaymentDetail entityData = JsonConvert.DeserializeObject<PaymentDetail>(JsonConvert.SerializeObject(entity));

                            result.Add(entityData);
                        }
                    }
                }
                conn.Close();
            }
            return result;
        }
        public int InsertByPaymentId(List<PaymentDetail> list, Guid paymentId)
        {
            var rowAffected = 0;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                foreach (var paymentDetail in list)
                {
                    var columnList = new List<string>();
                    var paramList = new List<string>();

                    var props = typeof(PaymentDetail).GetProperties();

                    using (var cmd = new NpgsqlCommand("", conn))
                    {
                        var errorMsg = new Dictionary<string, string>();
                        for (int i = 0; i < props.Length; i++)
                        {
                            var prop = props[i];
                            var propName = ToSnakeCase(prop.Name);
                            var propValue = prop.GetValue(paymentDetail);

                            if ((propName == "debit_account_id" || propName == "credit_account_id") && propValue == null)
                            {
                                var columnName = (propName == "debit_account_id") ? Resource.MISA_DebitAccount_Name : Resource.MISA_CreditAccount_Name;

                                errorMsg.Add(ToPascalCase(propName), $"{columnName} {Resource.VN_NoEmpty}");
                                throw new MISAException(Resource.VN_NoEmptyMsg_Common, errorMsg);
                            }

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


                        var sqlCommandString = $"INSERT INTO payment_detail ({columnString}) VALUES ({paramsString});";
                        cmd.CommandText = sqlCommandString;
                        var row = cmd.ExecuteNonQuery();

                        rowAffected += row;
                    }
                }
                conn.Close();
                return rowAffected;

            }
        }
    }
    #endregion
}

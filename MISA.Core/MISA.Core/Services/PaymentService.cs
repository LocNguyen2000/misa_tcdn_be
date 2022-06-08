using MISA.Web02.Core.Services;
using MISA.WEB02.Core.Interfaces;
using MISA.WEB02.Core.Entities;
using MISA.Web02.Core.Entities;
using PaymentDetail = MISA.Web02.Core.Entities.PaymentDetail;
using Npgsql;
using System.Transactions;

namespace MISA.Core.Services
{
    /// <summary>
    /// Service nhận dữ liệu đầu vào của API
    /// để validate
    /// </summary>
    /// CREATED BY NHLOC - 14/04/2022
    public class PaymentService : BaseService<Payment>, IPaymentService
    {
        #region DECLARE
        IPaymentRepository _paymentRepository;
        IPaymentDetailRepository _paymentDetailRepository;
        #endregion

        #region CONSTRUCTOR
        public PaymentService(IPaymentRepository paymentRepository, IPaymentDetailRepository paymentDetailRepository) : base(paymentRepository)
        {
            _paymentRepository = paymentRepository;
            _paymentDetailRepository = paymentDetailRepository;
        }
        #endregion

        #region METHODS
        

        public int InsertWithDetailService(PaymentDetailGroup paymentDetailGroup)
        {
            // Tạo mới payment id
            var newPaymentId = Guid.NewGuid();  

            // Gán id vào đối tượng mới để insert
            Payment payment = paymentDetailGroup.payment;
            payment.PaymentId = newPaymentId;

            // Tạo transaction scoped để khi lỗi > không insert vào
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    // thực hiện nghiệp vụ insert 
                    var result = InsertService(payment);

                    // tạo danh sách hạch toán mới 
                    List<PaymentDetail> detailPayments = paymentDetailGroup.paymentDetail;

                    if (detailPayments != null && detailPayments.Count > 0)
                    {
                        foreach (var detail in detailPayments)
                        {
                            // gán khóa ngoại vào từng hạch toán
                            detail.PaymentId = newPaymentId;
                        }
                        // insert nhiều hạch toán theo id khóa ngoại
                        _paymentDetailRepository.InsertByPaymentId(detailPayments, newPaymentId);
                    }

                    scope.Complete();

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}

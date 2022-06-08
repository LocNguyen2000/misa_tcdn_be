using MISA.Web02.Core.Entities;
using MISA.Web02.Core.Services;
using MISA.WEB02.Core.Interfaces;

namespace MISA.Core.Services
{
    /// <summary>
    /// Service nhận dữ liệu đầu vào của API
    /// để validate
    /// </summary>
    /// CREATED BY NHLOC - 14/04/2022
    public class PaymentDetailService : BaseService<PaymentDetail>, IPaymentDetailService
    {
        #region DECLARE
        IPaymentDetailRepository _paymentDetailRepository;
        #endregion

        #region CONSTRUCTOR
        public PaymentDetailService(IPaymentDetailRepository injection) : base(injection)
        {
            _paymentDetailRepository = injection;
        }
        #endregion

        #region METHODS
        
        #endregion
    }
}

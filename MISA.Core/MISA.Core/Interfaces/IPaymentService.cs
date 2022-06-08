using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.GD2.Core.Entities;

namespace MISA.WEB02.Core.Interfaces
{
    public interface IPaymentService : IBaseService<Payment>
    {
        /// <summary>
        /// Hàm thêm mới phiếu chi và bảng hạch toán chi tiết
        /// </summary>
        /// <param name="paymentDetailGroup"></param>
        /// <returns></returns>
        public int InsertWithDetailService(PaymentDetailGroup paymentDetailGroup);
    }
}

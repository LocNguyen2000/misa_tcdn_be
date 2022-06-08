using MISA.Web02.Core.Interfaces;
using MISA.Web02.Core.Entities;

namespace MISA.WEB02.Core.Interfaces
{
    public interface IPaymentDetailRepository : IBaseRepository<PaymentDetail>
    {
        /// <summary>
        /// Chèn nhiều dữ liệu hạch toán theo id chi tiêu
        /// </summary>
        /// <param name="list">danh sách bảng hạch toán</param>
        /// <param name="paymentId">khóa chính chi tiêu</param>
        /// <returns></returns>
        public int InsertByPaymentId(List<PaymentDetail> list, Guid paymentId);
        /// <summary>
        /// Lấy nhiều dữ liệu hạch toán theo id chi tiêu
        /// </summary>
        /// <param name="list">danh sách bảng hạch toán</param>
        /// <param name="paymentId">khóa chính chi tiêu</param>
        /// <returns></returns>
        public List<PaymentDetail> GetByPaymentId(Guid paymentId);
    }
}

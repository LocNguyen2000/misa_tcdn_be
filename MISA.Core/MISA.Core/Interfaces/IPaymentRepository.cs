using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.GD2.Core.Entities;

namespace MISA.WEB02.Core.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        /// <summary>
        /// Lấy dữ liệu chi tiêu + hạch toán
        /// </summary>
        /// <returns></returns>
        public PaymentDetailGroup GetPaymentWithDetail(Guid paymentId);
        /// <summary>
        /// lọc nâng cao dữ liệu payment
        /// </summary>
        /// <returns></returns>
        public object FilterAdvance(int pageIndex = 1, int pageSize = 10, string? filter = null, int? isRecord = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}

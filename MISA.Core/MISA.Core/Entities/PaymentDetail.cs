using MISA.Web02.Core.MISAAttribute;

namespace MISA.Web02.Core.Entities
{
    /// <summary>
    /// Chi tiết từng hạch toán trong phiếu chi
    /// </summary>
    public class PaymentDetail : BaseEntity
    {
        /// <summary>
        /// id accounting
        /// </summary>
        [PrimaryKey]
        public Guid PaymentDetailId { get; set; }

        /// <summary>
        /// diễn giải cho phiếu chi
        /// </summary>
        public string? DescriptionPaymentDetail { get; set; }

        /// <summary>
        /// khóa ngoại bảng Payment
        /// </summary>
        public Guid? PaymentId { get; set; }
        /// <summary>
        /// Tài khoản nợ
        /// </summary>
        public string? DebitAccountId { get; set; }
        /// <summary>
        /// số tk nợ
        /// </summary>
        [NotMappedProp]
        public string? DebitAccountNumber { get; set; }
        /// <summary>
        /// Tài khoản có
        /// </summary>
        public string? CreditAccountId { get; set; }
        /// <summary>
        /// số tk có
        /// </summary>
        [NotMappedProp]
        public string? CreditAccountNumber { get; set; }
        /// <summary>
        /// số tiền chi
        /// </summary>
        public decimal? CashAmount { get; set; }
        /// <summary>
        /// tỷ số thay đổi
        /// </summary>
        public decimal? ExchangeAmount { get; set; }
        /// <summary>
        /// khóa ngoại cho đối tượng(Vendor, employee,khách hàng ...)
        /// </summary>
        public Guid? AccountObjectId { get; set; }

        /// <summary>
        /// tên đối tượng(Vendor, employee,khách hàng ...)
        /// </summary>
        [NotMappedProp]
        public string? AccountObjectName { get; set; }

        /// <summary>
        /// mã đối tượng(Vendor, employee,khách hàng ...)
        /// </summary>
        [NotMappedProp]
        public string? AccountObjectCode { get; set; }
    }
}

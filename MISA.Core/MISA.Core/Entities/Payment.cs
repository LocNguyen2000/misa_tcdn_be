using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.Core.Entities
{
    /// <summary>
    /// Đối tượng cho phiếu chi tiền 
    /// </summary>
    public class Payment : BaseEntity
    {
        /// <summary>
        /// id payment
        /// </summary>
        public Guid PaymentId { get; set; }
        /// <summary>
        /// code payment
        /// </summary>
        public string? PaymentCode { get; set; }
        /// <summary>
        /// khóa ngoại bảng AccountObject
        /// </summary>
        // TODO: Payment.cs chưa biết đối tưởng ở đây dùng employee , vendor hay chỉ vendor
        public Guid? AccountObjectId { get; set; }
        /// <summary>
        /// tên, mã của AccountObject
        /// </summary>
        // TODO: Payment.cs chưa biết đối tưởng ở đây dùng employee , vendor hay chỉ vendor
        [NotMappedProp]
        public string? AccountObjectName { get; set; }
        [NotMappedProp]
        public string? AccountObjectCode { get; set; }
        /// <summary>
        /// tên Người nhận
        /// </summary>
        public string? ReceiverName { get; set; }
        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal? TotalPaymentMoney { get; set; }
        /// <summary>
        /// địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// ngày hạch toán
        /// </summary>
        public DateTime? AccountingDate { get; set; }
        /// <summary>
        /// Ngày Thanh toán
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// diễn giải cho phiếu chi(lí do chi)
        /// </summary>
        public string? DescriptionPayment { get; set; }
        /// <summary>
        /// Khóa ngoại bảng nhân viên
        /// </summary>
        public Guid? EmployeeId { get; set; }
        /// <summary>
        /// tệp đính kèm chứng từ gốc kèm theo
        /// </summary>
        public int? AttachDocumentAmount { get; set; }
        /// <summary>
        /// loại Tiền tệ
        /// </summary>
        public int? CurrencyId { get; set; }
        /// <summary>
        /// tỉ lệ quy đổi tiền tệ
        /// </summary>
        public decimal? ExchangeRate { get; set; }

    }
}

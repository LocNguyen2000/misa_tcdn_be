using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    /// <summary>
    /// Chi tiết từng hạch toán trong phiếu chi
    /// </summary>
    public class PaymentDetail
    {
        public Guid AccountingId { get; set; }
        public string? AccountingDetail { get; set; }
        public Guid PaymentId { get; set; }
        public string? DebitAccount { get; set; }
        public string? CreditAccount { get; set; }
        public decimal? CashAmount { get; set; }
        public Guid? DetailObjectId { get; set; }
    }
}

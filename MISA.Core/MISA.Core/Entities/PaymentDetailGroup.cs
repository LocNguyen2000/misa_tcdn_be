using MISA.Web02.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.Core.Entities
{
    /// <summary>
    /// Nhóm đối tượng chi tiêu + hạch toán
    /// </summary>
    public class PaymentDetailGroup
    {
        // dữ liệu chi tiêu
        public Payment payment { get; set; }
        // dữ liệu hạch toán
        public List<PaymentDetail>? paymentDetail { get; set; }
    }
}

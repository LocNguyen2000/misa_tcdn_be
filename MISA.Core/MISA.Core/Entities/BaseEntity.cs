using MISA.Web02.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web02.Core.Entities
{
    public class BaseEntity
    {
        [NotMappedProp]
        public DateTime? CreatedDate { get; set; }
        [NotMappedProp]
        public string? CreatedBy { get; set; }
        [NotMappedProp]
        public DateTime? ModifiedDate { get; set; }
        [NotMappedProp]
        public string? ModifiedBy { get; set; }

    }
}
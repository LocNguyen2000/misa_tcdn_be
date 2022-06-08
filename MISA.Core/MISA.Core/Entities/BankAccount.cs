using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;

namespace MISA.WEB02.Core.Entities
{
    public class BankAccount: BaseEntity
    {
        public Guid BankAccountId { get; set; }

        public string? BankName { get; set; }

        public string?  BankBranch { get; set; }
     
        public string? BankPlace { get; set; }

        public Guid VendorId { get; set; }

        
    }
}

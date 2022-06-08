using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;
using MISA.WEB02.Core.Resources;

namespace MISA.WEB02.Core.Entities
{
    public class Account
    {
        [PrimaryKey]
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string? AccountAttribute { get; set; }
        public string? AccountDescription { get; set; }
        public bool? AccountState { get; set; }
        [NotMappedProp]
        public string? AccountStateName 
        { 
            get
            {
                 switch (AccountState)
                 {
                    case true:
                        return "Đang sử dụng";
                    case false:
                        return "Ngừng sử dụng";
                    default:
                        return null;
                 }
            }
            set { } 
        }
        public string? AccountEnglishName { get; set; }
        public bool? IsParent { get; set; }
        public int? Grade { get; set; }
        public Guid? ParentId { get; set; }
    }
}

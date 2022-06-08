using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;

namespace MISA.Core.Entities
{
    public class Department : BaseEntity
    {
        [PrimaryKey]
        public Guid DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
    }
}

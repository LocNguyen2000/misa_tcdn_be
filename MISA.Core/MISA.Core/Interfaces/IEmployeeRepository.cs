using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Core.Entities;

namespace MISA.Core.Interfaces
{
    /// <summary>
    /// Interface định nghĩa hàm riêng
    /// xử lý tương tác CSDL của nhân viên
    /// </summary>
    /// CREATED BY NHLOC 13/04/2022
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        ///// <summary>
        ///// Xóa hàng loạt nhân viên
        ///// </summary>
        ///// <param name="employeeIds"></param>
        ///// <returns></returns>
        ///// CREATED BY NHLOC 20/04/2022
        //public int DeleteMultiple(List<Guid> employeeIds);

    }
}

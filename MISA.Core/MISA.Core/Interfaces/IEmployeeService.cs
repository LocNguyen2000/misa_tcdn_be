using MISA.Core.Entities;
using MISA.Web02.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    /// <summary>
    /// Interface định nghĩa hàm riêng
    /// xử lý nghiệp vụ validate dữ liệu đầu vào
    /// </summary>
    public interface IEmployeeService: IBaseService<Employee>
    {
        /// <summary>
        /// Nghiệp vụ validate đầu vào trước khi filter dữ liệu
        /// <param name="id">Khóa chính của nhân viên</param>
        /// <returns></returns>
        /// </summary>
        /// CREATED BY NHLOC 20/04/2022
        public object FilterService(int ?pageIndex, int ?pageSize, string? employeeFilter);
        /// <summary>
        /// Nghiệp vụ validate tất cả ID trước khi xóa hàng
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// CREATED BY NHLOC 20/04/2022
        public int DeleteMutiple(List<Guid> ids);
        /// <summary>
        /// Xuất file dữ liệu nhân viên
        /// </summary>
        /// <returns>
        ///  Dữ liệu file nhân viên
        /// </returns>
        /// CREATED BY NHLOC 25/04/2022
        public byte[] ExportData();
    }
}

using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using MISA.Web02.Core.Services;
using MISA.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    /// <summary>
    /// Service nhận dữ liệu đầu vào của API
    /// để validate
    /// </summary>
    /// CREATED BY NHLOC - 14/04/2022
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region DECLARE
        IEmployeeRepository _employeeRepository;
        #endregion
        /// <summary>
        /// Khởi tạo với Dependency Injection
        /// </summary>
        /// Created By NHLOC - 11/04/2022
        #region CONSTRUCTOR

        public EmployeeService (IEmployeeRepository injection) : base(injection)
        {
            _employeeRepository = injection;
        }
        #endregion

        #region IEmployeeSerrvice METHODS
        public int DeleteMutiple(List<Guid> ids)
        {
            // Lưu các thông báo lỗi
            var errorMsg = new Dictionary<string, string>();

            throw new NotImplementedException();
        }

        public byte[] ExportData()
        {
            var employeesData = _employeeRepository.Get();
            var result = EmployeeEPP.Export(employeesData);
            return result;
        }

        public object FilterService(int? pageIndex = 1, int? pageSize = 10, string ?employeeFilter=null)
        {
            // Lưu các thông báo lỗi
            var errorMsg = new Dictionary<string, string>();

            // Check lỗi đầu vào cho trang hiện tại và kích thước trang
            // Kiểm tra null hoặc âm
            pageIndex = (pageIndex == null || pageIndex < 0) ? 1 : pageIndex;
            //if ( pageIndex == null || pageIndex < 0)
            //{
            //    pageIndex = 1;
            //}
            // Kiểm tra null hoặc âm
            pageSize = (pageSize == null || pageSize < 0) ? 1 : pageSize;
            //if (pageSize == null || pageSize < 0)
            //{
            //    pageSize = 10;
            //}

            // tồn tại lỗi > gửi lỗi lên controller API
            if (errorMsg.Count > 0)
            {
                throw new MISAException("Dữ liệu đầu vào không hợp lệ", errorMsg);
            }

            // sau khi validate dữ liệu đầu vào > gửi cho repo để xóa
            var res = _employeeRepository.FilterData(pageIndex.Value, pageSize.Value, employeeFilter);
            return res;
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Exceptions;
using MISA.WEB02.Core.Resources;

namespace MISA.Web02.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : MISABaseController<Employee>
    {
        #region DECLARE
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region CONSTRUCTOR
        // Thực hiện Dependency Injection
        public EmployeesController(IEmployeeService empService, IEmployeeRepository empRepo): base(empService, empRepo)
        {
            _employeeService = empService;
            _employeeRepository = empRepo;
        }
        #endregion

        #region IBaseService METHODS
        /// GET: api/<EmployeeController>/
        /// <summary>
        /// Xuất file dữ liệu nhân viên
        /// </summary>
        /// CREATED BY NHLOC - 25/04/2022
        [HttpGet("export")]
        public IActionResult Export()
        {
            try
            {
                var file = _employeeService.ExportData();
                return File(file, "xlsx/xls", "output.xlsx");
            }
            catch (Exception ex)
            {
                var result = new MISAServiceResult
                {
                    UserMsg = Resource.VN_ErrorExceptionMsg,
                    DevMsg = ex.Message,
                    Data = ex.Data,
                };
                return StatusCode(500, result);
            }
        }
        #endregion
    }
}

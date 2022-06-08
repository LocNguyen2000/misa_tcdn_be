using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Web02.Core.Interfaces;

namespace MISA.Web02.API.Controllers
{
    public class DepartmentsController : MISABaseController<Department>
    {
        IDepartmentRepository _departmentRepository;
        IDepartmentService _departmentService;

        /// <summary>
        /// Thực hiện injection
        /// </summary>
        /// <param name="ibaseService"></param>
        /// <param name="ibaseRepository"></param>
        public DepartmentsController(IDepartmentService ibaseService, IDepartmentRepository ibaseRepository) : base(ibaseService, ibaseRepository)
        {
            _departmentRepository = ibaseRepository;
            _departmentService = ibaseService;
        }
    }
}

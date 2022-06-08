using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Web02.Infrastructor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    /// <summary>
    /// Lấy từ base repo dùng chung (Phòng ban) 
    /// </summary>
    /// CREATED BY NHLOC - 20/04/22
    public class DepartmentRepository: BaseRepository<Department>, IDepartmentRepository
    {
    }
}

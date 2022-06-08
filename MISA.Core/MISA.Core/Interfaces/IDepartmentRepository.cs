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
    /// Interface repository của phòng ban - từ interface base repo
    /// </summary>
    /// CREATED BY NHLOC - 20/04/22
    public interface IDepartmentRepository: IBaseRepository<Department>
    {
    }
}

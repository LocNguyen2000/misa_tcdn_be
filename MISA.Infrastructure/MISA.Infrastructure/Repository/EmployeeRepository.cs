using Dapper;
using MySqlConnector;
using System.Data;
using System.Reflection;

using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Web02.Core.MISAAttribute;
using MISA.Web02.Infrastructor.Repositories;
using System.Linq;
using Npgsql;
using MISA.WEB02.Core.Entities;

namespace MISA.Infrastructure.Repository
{
    /// <summary>
    /// Base Repo dùng chung (Nhân viên) + Interface repo của nhân viên
    /// </summary>
    /// CREATED BY: NHLOC - 14/04/2022
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region CONSTRUCTOR
        public EmployeeRepository() {}
        #endregion

        #region IEmployeeRepository METHODS
        

        #endregion
    }
}

using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;

namespace MISA.Core.Interfaces
{
    /// <summary>
    /// Interface repository - từ interface base repo
    /// </summary>
    /// CREATED BY NHLOC - 20/04/22
    public interface IAccountRepository: IBaseRepository<Account>
    {
        /// <summary>
        /// Lấy dữ liệu theo tài khoản cha 
        /// </summary>
        /// <returns></returns>
        public List<Account> GetByParent(string accountCode);

    }
}

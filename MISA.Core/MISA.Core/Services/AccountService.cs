using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using MISA.Web02.Core.Services;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        #region DECLARE
        IAccountRepository _accountRepository;
        #endregion

        #region CONSTRUCTOR
        public AccountService(IAccountRepository accountRepo) : base(accountRepo)
        {
            _accountRepository = accountRepo;
        }
       
        #endregion
    }
}

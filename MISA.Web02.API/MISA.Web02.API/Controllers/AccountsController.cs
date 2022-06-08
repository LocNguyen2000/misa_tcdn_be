using Microsoft.AspNetCore.Mvc;
using MISA.Core.Interfaces;
using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;

namespace MISA.Web02.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : MISABaseController<Account>
    {
        IAccountService _accountService;
        IAccountRepository _accountRepository;

        /// <summary>
        /// Thực hiện injection
        /// </summary>
        /// 
        public AccountsController(IAccountService accountService, IAccountRepository accountRepository) : base(accountService, accountRepository)
        {
            _accountService = accountService;
            _accountRepository = accountRepository;
        }
        /// <summary>
        /// Lấy mã code con theo code cha
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        [HttpGet("ByParent/{accountCode}")]
        public IActionResult GetByParentCode(string accountCode)
        {
            try
            {
                var result = _accountRepository.GetByParent(accountCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var err = HandleException(ex);
                return err;
            }
        }
    }
}

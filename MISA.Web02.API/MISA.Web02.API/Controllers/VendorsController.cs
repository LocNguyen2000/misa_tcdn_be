using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Interfaces;
using MISA.WEB02.Core.Resources;

namespace MISA.Web02.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VendorsController : MISABaseController<Vendor>
    {
        #region DECLARE
        private readonly IVendorService _vendorService;
        private readonly IVendorRepository _vendorRepository;
        #endregion

        #region CONSTRUCTOR
        // Thực hiện Dependency Injection
        public VendorsController(IVendorService vendorService, IVendorRepository vendorRepo) : base(vendorService, vendorRepo)
        {
            _vendorService = vendorService;
            _vendorRepository = vendorRepo;
        }
        #endregion

        #region IBaseService METHODS

        //[HttpPost("detail")]
        //public IActionResult PostWithDetail(VendorDetail vendorDetail)
        //{
        //    try
        //    {
        //        var vendor = vendorDetail.vendor;
        //        var banks = vendorDetail.bankAccounts;

        //        var result = _vendorRepository.InsertWithDetail(vendor, banks);
        //        if (result > 0)
        //        {
        //            return StatusCode(201, result);
        //        }
        //        else
        //        {
        //            return Ok(result);
        //        }
        //    }
        //    catch (MISAException ex)
        //    {
        //        return base.HandleException(ex);
        //    }
        //}
        #endregion
    }
}

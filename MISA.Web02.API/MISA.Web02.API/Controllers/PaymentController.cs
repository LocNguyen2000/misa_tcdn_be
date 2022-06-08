using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Web02.Core.Utilities;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Interfaces;
using MISA.WEB02.Core.Resources;
using MISA.WEB02.GD2.Core.Entities;
using Newtonsoft.Json.Linq;

namespace MISA.Web02.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : MISABaseController<Payment>
    {
        #region DECLARE
        private readonly IPaymentService _paymentService;
        private readonly IPaymentRepository _paymentRepository;
        #endregion

        #region CONSTRUCTOR
        // Thực hiện Dependency Injection
        public PaymentsController(IPaymentService paymentService, IPaymentRepository paymentRepository) : base(paymentService, paymentRepository)
        {
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
        }
        #endregion

        #region METHODS
        [HttpGet("PaymentWithDetail/{paymentId}")]
        public IActionResult GetWithDetails(Guid paymentId)
        {
            try
            {
                var paymentDetailGroup = _paymentRepository.GetPaymentWithDetail(paymentId);
                return paymentDetailGroup == null ? NotFound() : Ok(paymentDetailGroup);
            }
            catch (Exception e)
            {
                var msg = HandleException(e);
                return (msg);
            }
        }

        [HttpPost("PaymentWithDetail")]
        public IActionResult PostWithDetails([FromBody] PaymentDetailGroup paymentDetailGroup)
        {
            try
            {
                var rowAffected = _paymentService.InsertWithDetailService(paymentDetailGroup);

                return Ok(rowAffected);
            }
            catch (Exception e)
            {
                var msg = HandleException(e);
                return (msg);
            }
        }
        [HttpGet("filterAdvance")]
        public IActionResult PagingAdvance([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null, [FromQuery] int? isRecord = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                // Query filter dữ liệu (kết quả trả về ở dạng JSON string)
                var dataListByPage = _paymentRepository.FilterAdvance(pageIndex, pageSize, filter, isRecord, startDate, endDate);

                // Parse ra sử dụng JObject của NewtonsoftJson
                var jObject = JObject.Parse(dataListByPage.ToString());

                // map các key của object trong kết quả filter về dạng CamelCase
                jObject.Capitalize();

                return Ok(jObject);
            }
            catch (Exception e)
            {
                var msg = HandleException(e);
                return (msg);
            }
        }
        #endregion


    }
}

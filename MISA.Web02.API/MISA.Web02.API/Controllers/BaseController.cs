using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Web02.Core.Interfaces;
using MISA.Web02.Core.Utilities;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Resources;
using Newtonsoft.Json.Linq;

namespace MISA.Web02.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class MISABaseController<T> : ControllerBase
    {
        #region DECLARE
        IBaseService<T> _baseService;
        IBaseRepository<T> _baseRepository;
        #endregion

        #region CONSTRUCTOR
        // Thực hiện dependency injection
        public MISABaseController(IBaseService<T> ibaseService, IBaseRepository<T> ibaseRepository)
        {
            _baseService = ibaseService;
            _baseRepository = ibaseRepository;
        }
        #endregion

        #region BASE CONTROLLER METHODS
        /// <summary>
        /// API GET - trả về tất cả dữ liệu của entity
        /// </summary>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        [HttpGet]
        public virtual IActionResult Get()
        {
            try
            {
                var entities = _baseRepository.Get();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }
        /// <summary>
        /// Lấy dữ liệu nhan viên theo id
        /// </summary>
        /// <param name="id">khóa chính của entity</param>
        [HttpGet("{id}")]
        public virtual IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var entity = _baseRepository.GetById(id);

                if (entity == null)
                {
                    return NoContent();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// API POST - thêm mới dữ liệu của entity
        /// </summary>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        [HttpPost]
        public virtual IActionResult Post([FromBody] T entity)
        {
            try
            {
                var res = _baseService.InsertService(entity);
                if (res > 0)
                {
                    return StatusCode(201, res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (MISAException ex)
            {
                return HandleException(ex);
            }

        }
        /// <summary>
        /// API PUT - cập nhật dữ liệu của entity
        /// </summary>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        [HttpPut("{id}")]
        public virtual IActionResult Put([FromRoute]  Guid id, [FromBody] T entity )
        {
            try
            {
                var res = _baseService.UpdateService(id, entity);
                return Ok(res);
            }
            catch (MISAException ex)
            {
                ///Ghi log vào hệ thống
                ///
                var result = new MISAServiceResult
                {
                    UserMsg = Resource.VN_ErrorExceptionMsg,
                    DevMsg = ex.Message,
                    Data = ex.Data,
                };

                return StatusCode(400, result);
            }
        }
        /// <summary>
        /// API DELETE - xóa dữ liệu của entity
        /// </summary>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        [HttpDelete("{entityId}")]
        public virtual IActionResult Delete([FromRoute] Guid entityId)
        {
            try
            {
                var res = _baseService.DeleteService(entityId);
                if (res > 0)
                {
                    return Ok(res);
                }
                else
                    return NotFound();
            }
            catch (MISAException ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Hàm base xử ly các exception 
        /// </summary>
        /// <param name="ex">các loại exception</param>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is MISAException)
            {
                var result = new MISAServiceResult
                {
                    UserMsg = Resource.VN_ErrorExceptionMsg,
                    DevMsg = ex.Message,
                    Data = ex.Data,
                };

                return StatusCode(400, result);
            }
            else
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

        /// GET: api/<BaseController>
        /// <summary>
        /// Lấy mã nhân viên mới về
        /// </summary>
        [HttpGet("NewCode")]
        public virtual IActionResult GetNewCode()
        {
            try
            {
                // Lấy dữ liệu tất cả nhân viên
                var result = _baseRepository.GetNewCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Ghi lỗi vào hệ thống
                var result = new MISAServiceResult
                {
                    UserMsg = Resource.VN_ErrorExceptionMsg,
                    DevMsg = ex.Message,
                    Data = ex.Data,
                };
                return StatusCode(500, result);
            }
        }

        /// <summary>
        /// API filter dữ liệu và phân trang 
        /// </summary>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize"> kích thước trang</param>
        /// <param name="filter">filter nhà cung cấp theo mã hoặc tên</param>
        /// <returns></returns>
        /// CREATED BY NHLOC - 20/04/2022
        [HttpGet("filter")]
        public virtual IActionResult Paging([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
        {
            try
            {
                // Query filter dữ liệu (kết quả trả về ở dạng JSON string)
                var dataListByPage = _baseRepository.FilterData(pageIndex, pageSize, filter);
                
                // Parse ra sử dụng JObject của NewtonsoftJson
                var jObject = JObject.Parse(dataListByPage.ToString());

                // map các key của object trong kết quả filter về dạng CamelCase
                jObject.Capitalize();

                return Ok(jObject);
            }
            catch (Exception ex)
            {
                var error = HandleException(ex);
                return error;
            }
        }
        /// DELETE: api/<BaseController>/
        /// <summary>
        /// Xóa nhiều đối tượng theo id
        /// </summary>
        /// <paramref name="entityIdS"> Danh sách khóa chính của đối tượng </paramref>
        /// CREATED BY NHLOC - 23/04/2022
        [HttpDelete("DeleteMany")]
        public IActionResult DeleteMultiple([FromBody] List<Guid> entityIds)
        {
            try
            {
                // Xóa nhiều
                var res = _baseRepository.DeleteMultiple(entityIds);
                return Ok(res);
            }
            catch (Exception ex)
            {
               var error = HandleException(ex);
                return error;
            }
        }
        /// <summary>
        /// Xuất file dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpPost("Export")]
        public virtual IActionResult Export([FromBody] List<TableOption> ColumnList)
        {
            try
            {
                // Xuất file theo các dòng được hiển thị 
                var file = _baseService.ExportData(ColumnList);
                return File(file, "xlsx/xls", "output.xlsx");
            }
            catch (Exception ex )
            {
                var error = HandleException(ex);
                return error;
            }
        }
        #endregion
    }
}
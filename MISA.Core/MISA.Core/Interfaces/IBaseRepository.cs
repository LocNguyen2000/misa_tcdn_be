using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web02.Core.Interfaces
{
    /// <summary>
    /// Interface dùng chung cho các entity
    /// </summary>
    /// <typeparam name="T">Loại entity được truyền vào</typeparam>
    /// CREATED BY NHLOC 13/04/2022
    public interface IBaseRepository<T>
    {
        /// <returns >Mã đối tượng mới nhất</returns>
        /// <summary>
        /// lấy mã đối tượng mới nhất
        /// </summary>
        /// CREATED BY NHLOC 20/04/2022
        public string GetNewCode();
        /// <summary>
        /// Lấy dữ liệu entity vào DB
        /// </summary>
        /// <returns >Dữ liệu entity</returns>
        /// CREATED BY NHLOC 13/04/2022
        public IEnumerable<T> Get();
        /// <summary>
        /// Lấy dữ liệu entity theo khóa chính vào DB
        /// </summary>
        /// <param name="entityId">ID khóa chính entity</param>
        /// <returns >Dữ liệu entity || Dữ liệu rỗng </returns>
        /// CREATED BY NHLOC 13/04/2022
        public T GetById(Guid entityId);
        /// <summary>
        /// Thêm mới entity trong DB
        /// </summary>
        /// <param name="entity">dữ liệu thêm mới</param>
        /// <returns >Số lượng dòng được thêm mới</returns>
        /// CREATED BY NHLOC 13/04/2022
        public int Insert(T entity);
        /// <summary>
        /// Cập nhật entity trong DB
        /// </summary>
        /// <param name="entityId">ID khóa chính entity</param>
        /// <param name="entity">Dữ liệu entity cần thay đổi</param>
        /// <returns>Số lượng dòng thay đổi</returns>
        /// CREATED BY NHLOC 13/04/2022
        public int Update(Guid entityId, T entity);
        /// <summary>
        /// Xóa entity trong DB
        /// </summary>
        /// <param name="entityId">Xóa theo khóa chính</param>
        /// <returns>Số lượng dòng bị xóa</returns>
        /// CREATED BY NHLOC 13/04/2022
        public int Delete(Guid entityId);
        /// <summary>
        /// Lọc dữ liệu trong db theo trang
        /// </summary>
        /// <param name="pageIndex"> số thứ tự trang </param>
        /// <param name="pageSize"> số dữ liệu / trang</param>
        /// <param name="filter"> bộ lọc (theo mã, tên, sđt)</param>
        /// <returns></returns>
        public object FilterData (int pageIndex, int pageSize, string? filter);
        /// <summary>
        /// Xóa hàng loạt dữ liệu
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        /// CREATED BY NHLOC 20/04/2022
        public int DeleteMultiple(List<Guid> entityIds);
        /// <summary>
        /// Kiểm tra Id khóa chính entity tồn tại
        /// </summary>
        /// <param name="entityId">ID khóa chính entity</param>
        /// <returns>True nếu tồn tại id | False nếu không tồn tại id</returns>
        /// CREATED BY NHLOC 13/04/2022
        public bool CheckExistId(Guid entityId);
        /// <summary>
        /// Kiểm tra mã entity tồn tại
        /// </summary>
        /// <param name="entityCode"> mã của entity</param>
        /// <returns>True nếu tồn tại mã | False nếu không tồn tại mã</returns>
        /// CREATED BY NHLOC 13/04/2022
        public bool CheckExistCode(string entityCode);
    }
}
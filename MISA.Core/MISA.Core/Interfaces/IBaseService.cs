using MISA.WEB02.Core.Entities;

namespace MISA.Web02.Core.Interfaces
{
    /// <summary>
    /// Base service interface dùng chung cho các entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T>
    {
        /// <summary>
        /// Nghiệp vụ validate đầu vào trước khi thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu entity được thêm mới</param>
        /// <returns>Số lượng dòng được thay đổi</returns>
        /// Created By NHLOC - 13/04/2022
        public int InsertService(T entity);
        /// <summary>
        /// Nghiệp vụ validate đầu vào trước khi cập nhật dữ liệu 
        /// </summary>
        /// <param name="entityId"> khóa chính của entity</param>
        /// <param name="entity">dữ liệu entity cập nhật</param>
        /// <returns>Số lượng dòng được thay đổi</returns>
        /// Created By NHLOC - 14/04/2022
        public int UpdateService(Guid entityId, T entity);
        /// <summary>
        /// Nghiệp vụ validate đầu vào trước khi xóa dữ liệu
        /// </summary>
        /// <param name="id">Khóa chính của entity</param>
        /// <returns>Số lượng dòng được thay đổi</returns>
        /// Created By NHLOC - 14/04/2022
        /// <summary>
        public int DeleteService(Guid entityId);

        /// <summary>
        /// Nghiệp vụ check mã đối tượng có đúng
        /// </summary>
        /// <param name="entityCode">Mã nhân viên</param>
        /// <returns></returns>
        public bool ValidateValidCode(string entityCode);

        /// <summary>
        /// Xuất file theo các cột được hiển thị
        /// </summary>
        /// <param name="ColumnList">danh sách các cột </param>
        /// <returns></returns>
        public byte[] ExportData(List<TableOption> ColumnList);
    }
}
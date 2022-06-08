using MISA.Core.Exceptions;
using MISA.Web02.Core.Interfaces;
using MISA.Web02.Core.MISAAttribute;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Resources;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using Newtonsoft.Json;
using System.Reflection;

namespace MISA.Web02.Core.Services
{
    public class BaseService<T> : IBaseService<T>
    {
        #region DECLARE
        readonly IBaseRepository<T> _baseRepository;
        readonly string _className;
        #endregion

        #region CONSTRUCTOR
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
            _className = typeof(T).Name;
        }
        #endregion

        #region IBaseService METHODS
        public int InsertService(T entity)
        {
            var entityCodeColumn = $"{_className}Code";

            // 1. Validate chung
            Dictionary<string, string> errorMsg = ValidateObject(entity);
            if (errorMsg.Count() > 0)
            {
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            // 2. Check code entity đã tồn tại
            // 2.1. Lấy code của entity
            var entityCode = typeof(T).GetProperty(entityCodeColumn).GetValue(entity, null).ToString();

            // 2.2. Check mã entity truyền vào tồn tại 
            var isExistCode = _baseRepository.CheckExistCode(entityCode);
            if (isExistCode)
            {
                // Mã đối tượng đã tồn tại
                errorMsg.Add(entityCodeColumn, $"Mã <{entityCode}> {Resource.VN_ExistData}");
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            // 2.3. Kiểm tra format của mã đối tượng
            var isValidCode = ValidateValidCode(entityCode);
            if (!isValidCode)
            {
                // Báo lỗi sai định dạng mã
                errorMsg.Add(entityCodeColumn, $"Mã {Resource.VN_ErrorFormatCode}");
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            //Dữ liệu đã hơp lệ thì thực hiện thêm mới
            var rowAdds = _baseRepository.Insert(entity);
            return rowAdds;
        }

        public virtual int UpdateService(Guid entityId, T entity)
        {
            var errorMsg = new Dictionary<string, string>();

            // 1. Validate chung
            errorMsg = ValidateObject(entity);

            if (errorMsg.Count() > 0)
            {
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }
            var entityIdColumn = $"{_className}Id";
            var entityCodeColumn = $"{_className}Code";

            // 2. Validate Id tồn tại
            var entityInDb = _baseRepository.GetById(entityId);
            if (entityInDb == null)
            {
                errorMsg.Add(entityIdColumn, $"Đối tượng {Resource.VN_NotExistData}");
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            // 3. Validate mã entity ở request ~ entity trong DB có trùng ko
            // 3.1. Lấy code của entity truyền vào ở body
            var entityCode = typeof(T).GetProperty(entityCodeColumn)?.GetValue(entity, null)?.ToString();
            var entityInDbCode = typeof(T).GetProperty(entityCodeColumn)?.GetValue(entityInDb, null)?.ToString();

            // 3.2. So sánh 2 code có trùng (trùng thì pass)
            if (entityInDbCode != entityCode)
            {
                // Không trùng >> check code trong request có tồn tại trong db không
                var existCodeInReq = _baseRepository.CheckExistCode(entityCode);

                // tồn tại >> báo lỗi
                if (existCodeInReq == true)
                {
                    // Báo lỗi mã đã tồn tại trong resource
                    errorMsg.Add(entityCodeColumn, $"Mã <{entityCode}> {Resource.VN_ExistData}");
                    throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
                }
            }
            // 3.3. Kiểm tra format của mã đối tượng
            var isValidCode = ValidateValidCode(entityCode);
            if (!isValidCode)
            {
                // Báo lỗi sai định dạng mã trong resource
                errorMsg.Add(entityCodeColumn, $"Mã <{entityCode}> {Resource.VN_ErrorFormatCode}");
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            //Thực hiện thêm mới dữ liệu vào database
            var rowUpdates = _baseRepository.Update(entityId, entity);
            return rowUpdates;
        }
        public virtual int DeleteService(Guid entityId)
        {
            Dictionary<string, string> errorMsg = new Dictionary<string, string>();

            var entityIdColumn = $"{_className}Id";

            // 1. Validate Id tồn tại
            var isExistId = _baseRepository.CheckExistId(entityId);
            if (!isExistId)
            {
                // Báo lỗi không tồn tại ID đối tượng
                errorMsg.Add(entityIdColumn, $"ID {Resource.VN_NotExistData}");
                throw new MISAException(Resource.VN_NotValidRequest, errorMsg);
            }

            var rowsAffect = _baseRepository.Delete(entityId);
            return rowsAffect;
        }
        /// <summary>
        /// kiểm tra dữ liệu dùng chung
        /// </summary>
        /// <param name="entity">Đối tượng entity cần để check</param>
        /// <returns>
        ///     Danh sách lỗi
        /// </returns>
        /// CREATE BY NHLOC - 25/04/2022
        public Dictionary<string, string> ValidateObject(T entity)
        {
            var errorMsg = new Dictionary<string, string>();

            //Lấy ra các prop:
            var properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                // Lấy tên, giá trị từng thuộc tính
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);

                // Nếu cột đó có để attribute check trống thì kiểm tra
                var isNotEmptyProp = prop.IsDefined(typeof(NotEmpty), true);
                if (isNotEmptyProp)
                {
                    // Xem property đó thuộc Guid >> Check rỗng
                    if (prop.IsDefined(typeof(IsGuid), true) && propValue.Equals(Guid.Empty))
                    {
                        // Rỗng thì thêm text báo lỗi bằng errorMsg của NotEmpty
                        var propAttribute = Attribute.GetCustomAttributes(prop, typeof(NotEmpty)).FirstOrDefault();
                        var errorContent = (propAttribute as NotEmpty)?.ErrorMsg;

                        // Thêm vào list error để throw lên
                        errorMsg.Add(propName, errorContent);
                    }

                    else if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                    {
                        // Rỗng thì thêm text báo lỗi bằng errorMsg của NotEmpty
                        var propAttribute = Attribute.GetCustomAttributes(prop, typeof(NotEmpty)).FirstOrDefault();
                        var errorContent = (propAttribute as NotEmpty)?.ErrorMsg;

                        // Thêm vào list error để throw lên
                        errorMsg.Add(propName, errorContent);
                    }
                }
            }
            return errorMsg;
        }


        public virtual byte[] ExportData(List<TableOption> ColumnList)
        {
            // lấy hết dữ liệu
            var dataList = _baseRepository.Get();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(new FileInfo("D:\\C-Disks\\Downloads")))
            {
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách xuất dữ liệu";

                //thêm 1 sheet để làm việc với tệp excel
                excelPackage.Workbook.Worksheets.Add("Danh sách xuất dữ liệu");
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];

                // Dòng chứa các tên cột
                int index = 1;
                int cells = 1;
                
                // cột số thứ tự
                workSheet.Cells[index, cells].Value = "STT";
                workSheet.Cells[index, cells].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[index, cells].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[index, cells].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[index, cells].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[index, cells].Style.Font.Bold = true;//In đậm
                workSheet.Cells[index, cells].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, cells].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BBB"));
                workSheet.Column(cells).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                // các cột còn lại
                foreach (var column in ColumnList)
                {
                    workSheet.Column(cells).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cells++;
                    
                    workSheet.Cells[index, cells].Value = column.Label;
                    workSheet.Cells[index, cells].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, cells].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, cells].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, cells].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, cells].Style.Font.Bold = true;//In đậm
                    workSheet.Cells[index, cells].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, cells].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BBB"));
                }
                workSheet.Row(index).Height = 20;

                //Dòng chứa dữ liệu (2-n) 
                index = 2;
                cells = 2;
                int count = 1;

                // for từng dòng
                foreach (var data in dataList)
                {
                    workSheet.Cells[index, 1].Value = count;
                    workSheet.Cells[index, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[index, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    // for từng cột
                    foreach (var column in ColumnList)
                    {
                        var prop = data.GetType().GetProperty(column.Name);

                        // Check nếu dữ liệu null >> để trống
                        var propValue = prop.GetValue(data, null);
                        if (propValue == null)
                        {
                            workSheet.Cells[index, cells].Value = "";
                        }
                        //  Không thì gán vào cột
                        else
                        {
                            // check kiểu datetime
                            if (propValue.GetType() == typeof(DateTime))
                            {
                                // format dữ liệu
                                workSheet.Cells[index, cells].Value = ((DateTime)propValue).ToString("dd/MM/yyyy");
                            }
                            // kiểu số liệu
                            else if (propValue.GetType() == typeof(float))
                            {
                                // thêm .00 
                                workSheet.Cells[index, cells].Style.Numberformat.Format = "#,##0.00";
                                workSheet.Cells[index, cells].Value = propValue;
                            }
                            // các kiểu khác gán bth
                            else
                            {
                                workSheet.Cells[index, cells].Value = propValue;
                            }
                        }
                        workSheet.Cells[index, cells].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[index, cells].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[index, cells].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[index, cells].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        cells++;
                    }
                    // quay lại cột đầu (ko tính stt)
                    cells = 2;
                    count++;
                    index++;
                }

                //tự động dãn cột
                workSheet.Cells.AutoFitColumns();

                //return file
                var file = excelPackage.GetAsByteArray();

                //dừng ile excel
                excelPackage.Dispose();

                return file;
            }
        }

        #endregion

        #region HÀM TIỆN ÍCH
        /// <summary>
        /// Hàm biến đổi string từ snake_case > PascalCase
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public string ToPascalCase(string str)
        {
            string result = Regex.Replace(str, "_[a-z]", delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });

            result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }
        public bool ValidateValidCode(string entityCode)
        {
            // Kiểm tra regex của mã nhân viên và mã đơn vị
            var regex = new Regex(@"^((\bNCC)|(\bDP)|(\bPM)|(\bNV-))\d[0-9]{5,8}$");

            if (!regex.IsMatch(entityCode.ToUpper()))
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// Hàm biến đổi string thường thành snake_case
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ToSnakeCase(string s)
        {
            string result = Regex.Replace(s, "[A-Z]", "_$0").ToLower();

            result = result[1..];

            return result;
        }


        #endregion
    }
}
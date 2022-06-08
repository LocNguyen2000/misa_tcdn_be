using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Core.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MISA.Core.Utilities
{
    public class EmployeeEPP
    {
        public static byte[] Export(IEnumerable<Employee> employeeList)
        {
            using (var excelPackage = new ExcelPackage(new FileInfo("D:\\C-Disks\\Downloads")))
            {
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách nhân viên";

                //thêm 1 sheet để làm việc với tệp excel
                excelPackage.Workbook.Worksheets.Add("Danh sách nhân viên");
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];

                //TIÊU đề của sheet
                workSheet.Cells["A1:J1"].Merge = true; // Nối cột
                workSheet.Cells["A1:J1"].Value = "DANH SÁCH NHÂN VIÊN"; // Tiêu đề của file excel
                workSheet.Cells["A1:J1"].Style.Font.Size = 18; // Kích thước font

                // Tạo trống cách tiêu đề vs bảng
                workSheet.Cells["A2:J2"].Merge = true; // Nối cột

                //header cho bảng
                workSheet.Cells["A3"].Value = "STT";
                workSheet.Cells["B3"].Value = "Mã nhân viên";
                workSheet.Cells["C3"].Value = "Tên nhân viên";
                workSheet.Cells["D3"].Value = "Giới tính";
                workSheet.Cells["E3"].Value = "Ngày sinh";
                workSheet.Cells["F3"].Value = "Chức danh";
                workSheet.Cells["G3"].Value = "Tên đơn vị";
                workSheet.Cells["H3"].Value = "Số tài khoản";
                workSheet.Cells["I3"].Value = "Tên ngân hàng";
                workSheet.Cells["J3"].Value = "Chi nhánh";


                int count = 0; // Vị trí STT
                int index = 3; // Vị trí bắt đầu của dữ liệu
                //thêm các sheet chi tiết
                foreach (var item in employeeList)
                {
                    count++;
                    index++;
                    //stt
                    workSheet.Cells[index, 1].Value = count;
                    //mã Nhân viên
                    workSheet.Cells[index, 2].Value = item.EmployeeCode == null ? "" : item.EmployeeCode.Trim();
                    //tên nhân viên
                    workSheet.Cells[index, 3].Value = item.EmployeeName == null ? "" : item.EmployeeName.Trim();
                    //giới tính
                    workSheet.Cells[index, 4].Value = item.GenderName == null ? "" : item.GenderName.Trim();
                    //ngày sinh
                    workSheet.Cells[index, 5].Value = item.DateOfBirth == null ? "" : ((DateTime)item.DateOfBirth).ToString("dd/MM/yyyy");
                    //chức danh
                    workSheet.Cells[index, 6].Value = item.PositionName == null ? "" : item.PositionName.Trim();
                    //tên đơn vị
                    workSheet.Cells[index, 7].Value = item.DepartmentName == null ? "" : item.DepartmentName.Trim();
                    //số tài khoản
                    workSheet.Cells[index, 8].Value = item.BankAccount == null ? "" : item.BankAccount.Trim();
                    //tên ngân hàng
                    workSheet.Cells[index, 9].Value = item.BankName == null ? "" : item.BankName.Trim();
                    //chi nhánh
                    workSheet.Cells[index, 10].Value = item.BankBranch == null ? "" : item.BankBranch.Trim();

                }
                //format file
                workSheet.Cells[$"A1:J{index}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A1:J{index}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A1:J{index}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A1:J{index}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                
                //fomat dữ liệu
                workSheet.Cells[$"A1:J{index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//căn trái
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//  căn giữa
                
                //set căn giữa hàng cho các cột
                workSheet.Cells[$"A1:J{index}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//căn giữa dòng
                
                //thêm dữ liệu các cột
                workSheet.Cells["A3:J3"].Style.Font.Bold = true;//In đậm
                workSheet.Cells["A3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A3:J3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BBB"));//background-color
                workSheet.Row(3).Height = 20;//độ rộng của cột header
                workSheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;//căn giữa chiều dọc tiêu dề
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//căn giữa chiều ngang tiêu đề
                
                //tự động dãn cột
                workSheet.Cells.AutoFitColumns();

                //return file
                var file = excelPackage.GetAsByteArray();

                //dừng ile excel
                excelPackage.Dispose();

                return file;
            }
        }
    }
}

using MISA.Core.Enum;
using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;
using MISA.WEB02.Core.Resources;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Thông tin của nhân viên
    /// </summary>
    /// Created by Ng Huu Loc 13-04-2022
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Id nhân viên khóa chính: bắt buộc
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Mã nhân viên: bắt buộc
        /// </summary>
        [DisplayName("Mã nhân viên")]
        [NotEmpty(isError: "Mã nhân viên không được để trống")]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Họ và tên 
        /// </summary>
        [NotMappedProp]
        public string? FirstName { get; set; }
        [NotMappedProp]
        public string? LastName { get; set; }
        /// <summary>
        /// Tên nhân viên: bắt buộc
        /// </summary>
        [DisplayName("Họ và tên")]
        [NotEmpty(isError: "Họ và tên không được để trống")]
        public string EmployeeName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Giới tính: (1-nam, 0-nữ, 2-khác)
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// Tên giới tính (Nam - nữ - khác)
        /// </summary>
        [NotMappedProp]
        public string? GenderName {
            get
            {
                switch (Gender)
                {
                    case (int)GenderEnum.Male:
                        return Resource.MISA_GenderName_Male;
                    case (int)GenderEnum.Female:
                        return Resource.MISA_GenderName_Female;
                    case (int)GenderEnum.Other:
                        return Resource.MISA_GenderName_Other;
                    default:
                        return null;
                }
            }
            set { }
        }
        /// <summary>
        /// Số điện thoại cá nhân và cố định
        /// </summary>
        public string? PhoneNumber { get; set; }
        public string? LandlineNumber { get; set; }
        /// <summary>
        /// Email sử dụng
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? ContactAddress { get; set; }
        /// <summary>
        /// ID của phòng ban: bắt buộc
        /// Ma phong ban
        /// Ten phong ban
        /// </summary>
        [DisplayName("ID Phòng ban")]
        [NotEmpty(isError: "ID Phòng ban không được để trống")]
        [IsGuid()]
        public Guid DepartmentId { get; set; }
        [NotMappedProp]
        public string? DepartmentCode { get; set; }
        [NotMappedProp]
        public string? DepartmentName { get; set; }
        /// <summary>
        /// Tên chức danh
        /// </summary>
        public string? PositionName { get; set; }
        /// <summary>
        /// TK Ngan hàng
        /// Tên ngan hang
        /// Chi nhanh ngan hàng
        /// </summary>
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        /// <summary>
        /// Luong nhan vien
        /// </summary>
        public string? Salary { get; set; }
        /// <summary>
        /// TG vào công ty
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// Số CMND/ CCCD
        /// Thời gian làm CMND
        /// Noi cap
        /// </summary>
        public string? IdentityNumber { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Trạng thái làm việc 
        /// (0 - đang làm việc, 1-đã nghỉ việc, 2-đang nghỉ phép)
        /// </summary>
        public int? WorkStatus { get; set; }

        [NotMappedProp]
        public string? WorkStatusName { 
            get
            {
                switch (WorkStatus)
                {
                    case (int) WorkStatusEnum.Working:
                        return Resource.MISA_WorkStatus_Working;
                    case (int) WorkStatusEnum.Stop:
                        return Resource.MISA_WorkStatus_Stop;
                    case (int) WorkStatusEnum.Holiday:
                        return Resource.MISA_WorkStatus_Holiday;
                    default:
                        return null;
                }
            }
            set { }
        }
    }
}

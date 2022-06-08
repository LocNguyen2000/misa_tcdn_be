using MISA.Web02.Core.Entities;
using MISA.Web02.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.Core.Entities
{
    public class Vendor : BaseEntity
    {
        [PrimaryKey]
        public Guid VendorId { get; set; }

        [NotEmpty("Mã nhà cung cấp không được để trống")]
        public string VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? VendorTaxCode { get; set; }
        public string? VendorPhoneNumber { get; set; }
        public float? DebitAmount { get; set; }
        public string? VendorAddress { get; set; }
        public string? Website { get; set; }
        /// <summary>
        /// Loại nhà cung cấp: 0 - tổ chức; 1 - cá nhân
        /// </summary>
        public int? VendorType { get; set; }
        /// <summary>
        /// Là khách hàng
        /// </summary>
        public Boolean? IsCustomer { get; set; }

        #region Liên hệ
        public string? ContactPrefixName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactLegalRep { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        #endregion

        #region Điều khoản
        public string? ContractId { get; set; }

        /// <summary>
        /// Số ngày nợ tối đa
        /// </summary>
        public int? MaxDebitDate { get; set; }

        /// <summary>
        /// Số nợ tối đa
        /// </summary>
        public float? MaxDebitAmount { get; set; }

        /// <summary>
        /// Tài khoản công nợ nhận
        /// </summary>
        public string? DebitReceiptAccountId { get; set; }


        /// <summary>
        /// Tài khoản công nợ trả
        /// </summary>
        public string? DebitPaymentAccountId { get; set; }

        /// <summary>
        /// Nhân viên mua hàng
        /// </summary>
        public Guid? EmployeeBuyerId { get; set; }

        /// <summary>
        /// Nhóm Khách hàng, nhà cung cấp
        /// </summary>
        public List<string>? ClientVendorGroupId { get; set; }
        #endregion

        #region Tài khoản ngân hàng
        public string? BankAccounts { get; set; }
        #endregion

        #region Địa chỉ khác
        public string? CountryId { get; set; }
        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? WardId { get; set; }
        public string? DeliveryAddresses { get; set; }
        #endregion

        #region Ghi chú
        public string? TextNote { get; set; }
        #endregion

        #region Cá nhân (thêm)
        public string? VendorPrefixName { get; set; }
        public string? LandlineNumber { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string? IdentityPlace { get; set; }

        #endregion

    }
}

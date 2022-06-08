using MISA.Web02.Core.Interfaces;
using MISA.WEB02.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.Core.Interfaces
{
    public interface IVendorRepository : IBaseRepository<Vendor>
    {
        /// <summary>
        /// Kiểm tra mã đối tượng NCC có tồn tại trong phiếu chi
        /// </summary>
        /// <param name="vendorId">Khóa chính của NCC</param>
        /// <returns>Mã NCC nếu tồn tại || Null nếu không</returns>
        public string GetVendorInPayment(Guid vendorId);
    }
}

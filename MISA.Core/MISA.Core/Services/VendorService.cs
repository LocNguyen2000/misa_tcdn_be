using MISA.Core.Exceptions;
using MISA.Web02.Core.Services;
using MISA.WEB02.Core.Entities;
using MISA.WEB02.Core.Interfaces;
using MISA.WEB02.Core.Resources;

namespace MISA.WEB02.Core.Services
{
    public class VendorService : BaseService<Vendor>, IVendorService
    {
        #region DECLARE
        IVendorRepository _vendorRepository;
        #endregion

        #region CONSTRUCTOR
        public VendorService(IVendorRepository injection) : base(injection)
        {
            _vendorRepository = injection;
        }
        public override int DeleteService(Guid entityId)
        {
            var errMsg = new Dictionary<string, string>();

            var vendorCodeInPayment = _vendorRepository.GetVendorInPayment(entityId);

            if (!string.IsNullOrEmpty(vendorCodeInPayment))
            {
                errMsg.Add("VendorCode", $"Nhà cung cấp <{vendorCodeInPayment}> {Resource.VN_ExistVendorInPayment}");
                throw new MISAException(Resource.VN_MethodNotAllowed, errMsg);
            }

            var rowsAffected = base.DeleteService(entityId);

            return rowsAffected;
        }
        #endregion
    }
}

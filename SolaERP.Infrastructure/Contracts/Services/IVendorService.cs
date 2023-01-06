using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Vendors;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IVendorService : ICrudService<Vendor>
    {
        public Task<ApiResponse<Vendor>> GetVendorDetails();

    }
}

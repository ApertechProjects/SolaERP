using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Venndors;
using SolaERP.Infrastructure.Entities.Vendors;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IVendorService : ICrudService<Vendors>
    {
        Task<ApiResponse<VendorInfoDto>> GetVendorByTaxIdAsync(string taxId);
    }
}

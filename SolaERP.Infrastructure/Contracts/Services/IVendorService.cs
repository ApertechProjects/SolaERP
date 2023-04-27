using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Services
{
    public interface IVendorService : ICrudService<Vendors>
    {
        Task<ApiResponse<VendorInfoDto>> GetVendorByTaxIdAsync(string taxId);
    }
}

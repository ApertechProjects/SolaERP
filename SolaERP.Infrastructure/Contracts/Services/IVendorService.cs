using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Services
{
    public interface IVendorService : ICrudService<Vendors>
    {
        Task<int> GetVendorByTaxIdAsync(string taxId);
    }
}

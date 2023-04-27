using SolaERP.Application.Entities.Supplier;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ISupplierRepository
    {
        Task<List<SupplierCode>> GetSupplierCodesAsync();
    }
}

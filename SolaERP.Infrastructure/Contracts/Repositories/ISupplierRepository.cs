using SolaERP.Infrastructure.Entities.Supplier;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ISupplierRepository
    {
        Task<List<SupplierCode>> GetSupplierCodesAsync();
    }
}

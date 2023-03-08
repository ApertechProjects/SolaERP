using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Supplier;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ISupplierService
    {
        Task<ApiResponse<List<SupplierCodeDto>>> GetSupplierCodesAsync();
    }
}

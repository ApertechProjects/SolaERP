using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Supplier;

namespace SolaERP.Application.Contracts.Services
{
    public interface ISupplierService
    {
        Task<ApiResponse<List<SupplierCodeDto>>> GetSupplierCodesAsync();
    }
}

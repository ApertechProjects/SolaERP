using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services;

public class OrderService: IOrderService
{
    public async Task<ApiResponse<string>> GetTypesAsync(int businessUnitId)
    {
        return null;
    }
}
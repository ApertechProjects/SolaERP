using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILayoutService
    {
        Task<ApiResponse<LayoutDto>> GetUserLayoutAsync(string name, string layoutKey);
        Task<ApiResponse<bool>> SaveLayoutAsync(string name, LayoutDto layout);
        Task<ApiResponse<bool>> DeleteLayoutAsync(string name, string key);
    }
}

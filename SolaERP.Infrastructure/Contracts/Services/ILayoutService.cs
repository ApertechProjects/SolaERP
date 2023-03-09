using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILayoutService
    {
        Task<ApiResponse<LayoutDto>> GetUserLayoutAsync(string finderToken, string layoutKey);
        Task<ApiResponse<bool>> SaveLayoutAsync(string finderToken, LayoutDto layout);
        Task<ApiResponse<bool>> DeleteLayoutAsync(string finderToken, LayoutDeleteModel layout);
    }
}

using SolaERP.Application.Dtos.Layout;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ILayoutService
    {
        Task<ApiResponse<LayoutDto>> GetUserLayoutAsync(string name, string layoutKey);
        Task<ApiResponse<bool>> SaveLayoutAsync(string name, LayoutDto layout);
        Task<ApiResponse<bool>> DeleteLayoutAsync(string name, string key);
    }
}

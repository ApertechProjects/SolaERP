using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Infrastructure.Services
{
    public interface ICrudService<T>
    {
        Task<ApiResponse<List<T>>> GetAllAsync();
        Task<T> AddAsync(T model);
        Task<ApiResponse<bool>> UpdateAsync(T model);
        Task<ApiResponse<bool>> RemoveAsync(T model);
    }
}

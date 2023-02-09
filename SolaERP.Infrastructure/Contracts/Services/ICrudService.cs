using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ICrudService<T>
    {
        Task<ApiResponse<List<T>>> GetAllAsync();
        Task AddAsync(T model);
        Task<ApiResponse<bool>> UpdateAsync(T model);
        Task<ApiResponse<bool>> RemoveAsync(int Id);
    }
}

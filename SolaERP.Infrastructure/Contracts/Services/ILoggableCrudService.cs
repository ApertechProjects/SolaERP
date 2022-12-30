using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILoggableCrudService<T>
    {
        Task<ApiResponse<List<T>>> GetAllAsync();
        Task<int> AddAsync(string authToken, T entity);
        Task<int> UpdateAsync(string authToken, T entity);
        Task<ApiResponse<bool>> RemoveAsync(int Id);
    }
}

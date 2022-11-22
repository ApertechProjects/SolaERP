using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Infrastructure.Services
{
    public interface ICrudService<T>
    {
        ApiResponse<List<T>> GetAll();
        Task<ApiResponse<bool>> AddAsync(T model);
    }
}

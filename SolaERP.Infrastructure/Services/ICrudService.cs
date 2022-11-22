using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Infrastructure.Services
{
    public interface ICrudService<T>
    {
        ApiResponse<List<T>> GetAll();
        Task<ApiResponse<Token>> AddAsync(T model);
    }
}

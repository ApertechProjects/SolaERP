using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Infrastructure.Services
{
    public interface IBaseService<T>
    {
        ApiResponse<List<T>> GetAll();
        ApiResponse<bool> Register(T model);
    }
}

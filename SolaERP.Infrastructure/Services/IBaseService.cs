using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Infrastructure.Services
{
    public interface IBaseService<T>
    {
        ApiResponse<List<T>> GetAll();
        ApiResponse<T> Register(T model);
    }
}

using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Application.Abstract
{
    public interface IBaseService<T>
    {
        ApiResponse<List<T>> GetAll();
        ApiResponse<T> Register(T model);
    }
}

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILogableCrudService<T>
    {
        Task<bool> AddAsync(T entity, int userId = default);
        void Update(T entity, int userId = default);
    }
}

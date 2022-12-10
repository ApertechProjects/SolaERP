namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILoggableCrudService<T>
    {
        Task<int> AddAsync(T entity, int userId);
        void Update(T entity, int userId);
    }
}

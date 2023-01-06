namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReadableAsync<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
    }
}

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReturnableServiceMethodAsync<T>
    {
        public Task<int> AddOrUpdate(T dto);
    }
}

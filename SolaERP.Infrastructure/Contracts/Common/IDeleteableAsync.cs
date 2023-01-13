namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IDeleteableAsync
    {
        public Task<int> DeleteAsync(int Id);
    }
}

namespace SolaERP.Application.Contracts.Common
{
    public interface IDeleteableAsync
    {
        public Task<int> DeleteAsync(int userId, int Id);
    }
}

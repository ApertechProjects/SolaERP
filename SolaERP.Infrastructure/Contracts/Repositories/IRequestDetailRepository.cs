using SolaERP.Infrastructure.Entities.Request;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<bool> RemoveRequestDetailAsync(int id);
    }
}

using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities.Request;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>, IReturnableRepoMethodAsync<RequestDetail>
    {
        bool RemoveRequestDetailAsync(int id);
    }
}

using SolaERP.Infrastructure.Entities.Request;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<List<RequestCardDetail>> GetRequestDetailsByMainIdAsync(int requestMainId);
    }
}

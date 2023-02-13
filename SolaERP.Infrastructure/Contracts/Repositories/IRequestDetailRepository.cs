using SolaERP.Infrastructure.Entities.Request;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<List<RequestDetail>> GetAllDetailsByRequestMainIdAsync(int requestMainId);
        Task<List<RequestCardDetail>> GetRequestDetailsByMainIdAsync(int requestMainId);
    }
}

using SolaERP.Infrastructure.Entities.Request;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<List<RequestDetail>> GetAllDetailsByRequestMainIdAsync(int requestMainId);
        Task<List<RequestDetailWithAnalysisCode>> GetRequestDetailsByMainIdWithAnalysisCodeAsync(int requestMainId);
    }
}

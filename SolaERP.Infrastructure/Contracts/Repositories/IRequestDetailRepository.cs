using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<List<RequestCardDetail>> GetRequestDetailsByMainIdAsync(int requestMainId);
        Task<RequestDetailApprovalInfo> GetDetailApprovalInfoAsync(int requestDetailId);
        Task<bool> RequestDetailChangeStatus(RequestDetailApproveModel model);
    }
}

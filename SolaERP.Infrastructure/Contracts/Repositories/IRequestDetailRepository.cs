using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRequestDetailRepository : ICrudOperations<RequestDetail>
    {
        Task<List<RequestCardDetail>> GetRequestDetailsByMainIdAsync(int requestMainId);
        Task<List<RequestDetailApprovalInfo>> GetDetailApprovalInfoAsync(int requestDetailId);
        Task<bool> RequestDetailChangeStatusAsync(int requestDetailId, int userId, int approveStatusid, string comment, int sequence, int rejectReasonId);
        Task<List<RequestCardAnalysis>> GetAnalysis(int requestMainId);
    }
}

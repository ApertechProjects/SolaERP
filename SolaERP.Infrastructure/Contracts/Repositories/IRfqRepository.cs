using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRfqRepository
    {
        Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter);
        Task<List<RfqAll>> GetAllAsync(RfqAllFilter filter);
        Task<List<SingleSourceReasonModel>> GetSingleSourceReasonsAsync();

        Task<int> AddMainAsync(RfqSaveCommandRequest request);
        Task<int> UpdateMainAsync(RfqSaveCommandRequest request);
        Task<int> DeleteMainsync(int id, int userId);


        Task<bool> AddDetailsAsync(RfqDetailsSaveRequestModel model);
        Task<bool> UpdateDetailsAsync(RfqDetailsSaveRequestModel model);
        Task<bool> DeleteDetailsAsync(int detailId);

        Task<List<RequestForRFQ>> GetRequestsForRfq(RFQRequestModel model);

    }
}

using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRfqRepository
    {
        Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter);
        Task<List<RfqAll>> GetAllAsync(RfqAllFilter filter);
        Task<List<SingleSourceReasonModel>> GetSingleSourceReasonsAsync();
        Task<List<RfqVendor>> GetVendorsForRfqAync(int businessCategoryId);

        Task<RFQMain> GetRFQMainAsync(int rfqMainId);
        Task<List<RFQDetail>> GetRFQDetailsAsync(int rfqMainId);
        Task<List<RFQRequestDetail>> GetRFQLineDeatilsAsync(int rfqMainId);

        Task<int> AddMainAsync(RfqSaveCommandRequest request);
        Task<int> UpdateMainAsync(RfqSaveCommandRequest request);
        Task<int> DeleteMainsync(int id, int userId);
        Task<List<SingleSourceReasonModel>> GetRFQSingeSourceReasons(int rfqMainId);

        Task<bool> AddDetailsAsync(List<RfqDetailSaveModel> Details, int mainId);
        Task<bool> SaveRFqRequestDetailsAsync(List<RfqRequestDetailSaveModel> details);
        Task<List<RequestForRFQ>> GetRequestsForRfq(RFQRequestModel model);
        Task<bool> ChangeRFQStatusAsync(RfqChangeStatusModel model, int userId);

    }
}

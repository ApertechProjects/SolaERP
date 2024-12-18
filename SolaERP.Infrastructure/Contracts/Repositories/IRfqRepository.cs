using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRfqRepository
    {
        Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter, int userId);
        Task<List<RfqAll>> GetAllAsync(RfqAllFilter filter);
        Task<List<SingleSourceReasonModel>> GetSingleSourceReasonsAsync();
        Task<List<RfqVendorToSend>> GetVendorsForRfqAync(int businessCategoryId);
        Task<List<RFQSingleSourceReasonsLoad>> GetSingleSourceReasons(int rfqMainId);

        Task<List<RFQInProgress>> GetInProgressesAsync(RFQFilterBase filter, int userId);

        Task<RFQMain> GetRFQMainAsync(int rfqMainId);
        Task<List<RFQDetail>> GetRFQDetailsAsync(int rfqMainId);
        Task<List<RFQRequestDetail>> GetRFQLineDeatilsAsync(int rfqMainId);

        Task<RfqSaveCommandResponse> AddMainAsync(RfqSaveCommandRequest request);
        Task<RfqSaveCommandResponse> UpdateMainAsync(RfqSaveCommandRequest request);
        Task<RfqSaveCommandResponse> DeleteMainsync(int id, int userId);
        Task<List<SingleSourceReasonModel>> GetRFQSingeSourceReasons(int rfqMainId);

        Task<bool> DetailsIUDAsync(List<RfqDetailSaveModel> Details, int mainId);
        Task<bool> RFQRequestDetailsIUDAsync(List<RfqRequestDetailSaveModel> details);
        Task<List<RequestForRFQ>> GetRequestsForRfq(RFQRequestModel model);
        Task<bool> ChangeRFQStatusAsync(RfqChangeStatusModel model, int userId);
        Task<Entities.UOM.Conversion> GetConversionAsync(int bussinessUnit, string itemCode);
        Task<List<UOM>> GetPUOMAsync(int businessUnitId, string itemCodes);
        Task<bool> RFQVendorIUDAsync(RFQVendorIUD vendorIUD, int userId);
        Task<List<int>> GetDetailIds(int id);
        Task<bool> ChangeRFQVendorResponseStatus(int rfqMainId, string vendorCode);
        Task<List<RFQVendors>> GetRfqVendors(int rfqMainId);

	}
}

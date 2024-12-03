using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UOM;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;


namespace SolaERP.Application.Contracts.Services
{
    public interface IRfqService
    {
        Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter,string userName);
        Task<ApiResponse<List<RfqAllDto>>> GetAllAsync(RfqAllFilter filter);
        Task<ApiResponse<List<BusinessCategory>>> GetBuCategoriesAsync();
        Task<ApiResponse<RfqSaveCommandResponse>> SaveRfqAsync(RfqSaveCommandRequest request, string userIdentity);
        Task<ApiResponse<List<SingleSourceReasonModel>>> GetSingleSourceReasonsAsync();
        Task<ApiResponse<bool>> DeleteAsync(List<int> rfqMainId, string userIdentity);
        Task<ApiResponse<List<RfqVendorToSend>>> GetRFQVendorsAsync(int buCategoryId);
        Task<ApiResponse<List<RequestRfqDto>>> GetRequestsForRFQ(string userIdentity, RFQRequestModel model);
        Task<ApiResponse<bool>> ChangeRFQStatusAsync(RfqChangeStatusModel model, string userIdentity);
        Task<ApiResponse<RFQMainDto>> GetRFQAsync(string userIdentity, int rfqMainId);
        Task<ApiResponse<List<RFQInProgressDto>>> GetInProgressAsync(RFQFilterBase filter, string userName);
        Task<ApiResponse<List<Dtos.RFQ.UOMDto>>> GetPUOMAsync(int businessUnitId, string itemCodes);
        Task<ApiResponse<int>> RFQVendorIUDAsync(RFQVendorIUDDto dto, string userIdentity);
        //Task<ApiResponse<SolaERP.Application.Dtos.UOM.ConversionDTO>> GetConversionAsync(int businessUnit, string itemCodes);
    }
}

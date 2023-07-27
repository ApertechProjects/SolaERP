using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IRfqService
    {
        Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter);
        Task<ApiResponse<List<RfqAllDto>>> GetAllAsync(RfqAllFilter filter);
        Task<ApiResponse<List<BusinessCategory>>> GetBuCategoriesAsync();
        Task<ApiResponse<int>> AddRfqMainAsync(RfqSaveCommandRequest request, string userIdentity);
        Task<ApiResponse<List<SingleSourceReasonModel>>> GetSingleSourceReasonsAsync();
        Task<ApiResponse<List<RequestRfqDto>>> GetRequestsForRFQ(string userIdentity, RFQRequestModel model);
    }
}

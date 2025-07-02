using Microsoft.AspNetCore.Http;
using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Mail;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
	public interface IRequestService : IDeleteableAsync
	{
		Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetModel requestMainGet, string name);
		Task<ApiResponse<List<RequestHeldDto>>> GetHeldAsync(RequestWFAGetModel requestMainGet, string name);
		Task<ApiResponse<List<RequestWFADto>>> GetWFAAsync(string name, RequestWFAGetModel requestWFAGetParametersDto);
		Task<bool> RemoveDetailAsync(int requestDetailId);
		Task<ApiResponse<List<RequestTypesDto>>> GetTypesAsync(int businessUnitId);

		Task<bool> ChangeMainStatusAsync(string name, int requestMainId, int approveStatus, string comment,
			int rejectReasonId,string businessUnitName,int? sequence,HttpResponse response);

		Task<bool> SendToApproveAsync(string name, List<int> requestMainIds,HttpResponse response);

		Task<ApiResponse<List<RequestMainDraftDto>>> GetDraftsAsync(RequestMainDraftModel getMainDraftParameters,
			string name);

		Task<ApiResponse<List<RequestAmendmentDto>>> GetChangeApprovalAsync(string name,
			RequestApproveAmendmentModel requestParametersDto);

		Task<ApiResponse<RequestCardMainDto>> GetByMainId(string name, int requestMainId, int businessUnitId);
		Task<ApiResponse<List<RequestApprovalInfoDto>>> GetApprovalInfoAsync(string name, int requestMainId);
		Task<ApiResponse<RequestMainDto>> GetHeaderAsync(string name, int requestMainId);
		Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetDetails(int requestmainId, int businessUnitId);

		Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateAsync(string name, HttpResponse response,
			RequestSaveModel model);

		Task<ApiResponse<bool>> DeleteAsync(string name, List<int> requestMainIds);
		Task<ApiResponse<List<RequestDetailApprovalInfoDto>>> GetDetailApprvalInfoAsync(int requestDetaildId);

		Task<bool> ChangeDetailStatusAsync(string name, int? requestDetailId, int approveStatusId, string comment,
			int? sequence, int rejectReasonId);

		Task ChangeDetailStatusAndSendMail(string userName, HttpResponse response, RequestDetailApproveModel model);
		Task<ApiResponse<bool>> UpdateBuyerAsync(RequestSetBuyer requestSetBuyer, string name);
		Task<ApiResponse<List<RequestFollowDto>>> GetFollowUsersAsync(int requestMainId);
		Task<ApiResponse<bool>> SaveFollowUserAsync(RequestFollowSaveModel saveModel);
		Task<ApiResponse<bool>> DeleteFollowUserAsync(int requestFollowId);
		Task<ApiResponse<int>> GetDefaultApprovalStage(string keyCode, int businessUnitId);
		Task<ApiResponse<List<RequestCategory>>> CategoryList(int businessUnitId, string keyCode);
		Task<ApiResponse<List<BuyersAssignmentDto>>> GetBuyersAssignment(RequestWFAGetModel model, string userName);
		Task<ApiResponse<bool>> Retrieve(RequestRetrieveDto data, string name);
		Task<ApiResponse<List<WarehouseInfoDto>>> GetWarehouseList(int businessUnitId);
	}
}
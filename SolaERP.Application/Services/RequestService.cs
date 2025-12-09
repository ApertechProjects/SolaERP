using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using SolaERP.Application.Enums;
using Microsoft.AspNetCore.Http;
using SolaERP.Application.Entities.General;
using System.Xml.Linq;
using SolaERP.Application.Dtos.Mail;
using SolaERP.Application.Dtos.UserDto;

namespace SolaERP.Persistence.Services
{
	public class RequestService : IRequestService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IRequestMainRepository _requestMainRepository;
		private readonly IRequestDetailRepository _requestDetailRepository;
		private readonly IUserRepository _userRepository;
		private readonly IUserService _userService;
		private readonly IMailService _mailService;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IAttachmentService _attachmentService;
		private readonly IBusinessUnitService _businessUnitService;
		private readonly IGeneralService _generalService;
		private readonly IApproveStageService _approveStageService;
		private readonly IBuyerService _buyerService;
		private readonly IBackgroundTaskQueue _taskQueue;



		public RequestService(IUnitOfWork unitOfWork,
			IMapper mapper,
			IRequestMainRepository requestMainRepository,
			IRequestDetailRepository requestDetailRepository,
			IUserRepository userRepository,
			IMailService mailService,
			IAttachmentService attachmentService,
			IBusinessUnitService businessUnitService,
			IGeneralService generalService,
			IEmailNotificationService emailNotificationService,
			IApproveStageService approveStageService,
			IBuyerService buyerService,
			IBackgroundTaskQueue taskQueue)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_requestMainRepository = requestMainRepository;
			_requestDetailRepository = requestDetailRepository;
			_userRepository = userRepository;
			_mailService = mailService;
			_attachmentService = attachmentService;
			_businessUnitService = businessUnitService;
			_generalService = generalService;
			_emailNotificationService = emailNotificationService;
			_approveStageService = approveStageService;
			_buyerService = buyerService;
			_taskQueue = taskQueue;

		}

		public async Task<bool> RemoveDetailAsync(int requestDetailId)
		{
			var result = await _requestDetailRepository.RemoveAsync(requestDetailId);
			await _unitOfWork.SaveChangesAsync();

			return result;
		}

		public async Task<ApiResponse<List<RequestMainDto>>> GetAllAsync(RequestMainGetModel model, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var mainRequest = await _requestMainRepository.GetAllAsync(model, userId);
			var mainRequestDto = _mapper.Map<List<RequestMainDto>>(mainRequest);
			return ApiResponse<List<RequestMainDto>>.Success(mainRequestDto);
		}

		public async Task<bool> SaveRequestDetailsAsync(RequestDetailDto requestDetailDto)
		{
			var entity = _mapper.Map<RequestDetail>(requestDetailDto);
			entity.RequestDate = entity.RequestDate.ConvertDateToValidDate();
			var requestDetails = await _requestDetailRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return requestDetails;
		}

		public async Task<bool> SaveRequestDetailsForStockAsync(RequestDetailForFromStockDto requestDetailDto)
		{
			var entity = _mapper.Map<RequestDetail>(requestDetailDto);
			var requestDetails = await _requestDetailRepository.SaveRequestByFromStock(entity);
			await _unitOfWork.SaveChangesAsync();
			return requestDetails;
		}

		public async Task<ApiResponse<List<RequestTypesDto>>> GetTypesAsync(int businessUnitId)
		{
			var entity = await _requestMainRepository.GetRequestTypesByBusinessUnitIdAsync(businessUnitId);
			var dto = _mapper.Map<List<RequestTypesDto>>(entity);

			return entity.Count > 0
				? ApiResponse<List<RequestTypesDto>>.Success(dto)
				: ApiResponse<List<RequestTypesDto>>.Fail("Request types not found", 404);
		}

		public async Task<bool> ChangeMainStatusAsync(string name, int requestMainId, int approveStatus, string comment,
			int rejectReasonId, string businessUnitName, int? sequence, HttpResponse response)
		{
			var userId = await _userRepository.ConvertIdentity(name);

			var result = await _requestMainRepository.RequestMainChangeStatusAsync(userId, requestMainId, approveStatus,
				comment, rejectReasonId);
			await _unitOfWork.SaveChangesAsync();

			if (result)
			{
				var stageCount = await _approveStageService.GetStageCountAsync(Procedures.Request);
				if (sequence == stageCount && approveStatus == 1)
				{
					var requestEmailSendUser = await RequestMailUsers(requestMainId);
					var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQA);
					await _mailService.SendMailForRequest(response, templates, requestEmailSendUser, EmailTemplateKey.REQA, sequence, businessUnitName);
				}
				else if (approveStatus == 2)
				{
					var requestEmailSendUser = await RequestMailUsers(requestMainId);
					var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQR);
					await _mailService.SendMailForRequest(response, templates, requestEmailSendUser, EmailTemplateKey.REQR, sequence, businessUnitName);
				}

			}


			return result;
		}


		public async Task<bool> SendToApproveAsync(string name, List<int> requestMainIds, HttpResponse response)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			User user = await _userRepository.GetByIdAsync(userId);
			string messageBody = "Request sent to approve by " + user.FullName;

			var sendToApproveTasks = requestMainIds.Select(async requestId =>
			{
				await _requestMainRepository.SendRequestToApproveAsync(userId, requestId);
				string businessUnitName = await _requestMainRepository.GetRequestBusinessUnitName(requestId);
				var requestEmailSendUser = await RequestMailUsers(requestId);

				var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQP);

				response.OnCompleted(async () =>
				{
					await _mailService.SendMailForRequest(response, templates, requestEmailSendUser, EmailTemplateKey.REQP, 1, businessUnitName);
				});

			}).ToList();

			await Task.WhenAll(sendToApproveTasks);
			await _unitOfWork.SaveChangesAsync();


			//bool allSuccess = sendToApproveTasks.All(task => task.res);
			return true;
		}

		private async Task<List<Application.Dtos.User.UserList>> RequestMailUsers(int requestMainId)
		{
			List<Application.Dtos.User.UserList> result = new();

			var requesterUser = await _requestMainRepository.RequesterMailInRequest(requestMainId);
			if (requesterUser.FullName is not null)
				result.Add(requesterUser);
			var buyerUser = await _requestMainRepository.BuyerMailInRequest(requestMainId);
			if (buyerUser.FullName is not null)
				result.Add(buyerUser);

			return result;
		}

		public async Task<ApiResponse<RequestCardMainDto>> GetByMainId(string name, int requestMainId,
			int businessUnitId)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var requestMain = await _requestMainRepository.GetRequesMainHeaderAsync(requestMainId, userId);

			if (requestMainId == 0)
				requestMain.BusinessUnitId = businessUnitId;

			requestMain.requestCardDetails =
				await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestMainId,
					requestMain.BusinessUnitId);
			requestMain.requestCardAnalysis = await _requestDetailRepository.GetAnalysis(requestMainId);
			var requestDto = _mapper.Map<RequestCardMainDto>(requestMain);
			requestDto.Attachments = await _attachmentService.GetAttachmentsAsync(requestDto.RequestMainId,
				SourceType.REQ, Modules.Request);
			return ApiResponse<RequestCardMainDto>.Success(requestDto);
		}

		public async Task<ApiResponse<List<RequestMainDraftDto>>> GetDraftsAsync(
			RequestMainDraftModel getMainDraftParameters, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);

			var mainDraftEntites =
				await _requestMainRepository.GetMainRequestDraftsAsync(getMainDraftParameters, userId);
			var mainDraftDto = _mapper.Map<List<RequestMainDraftDto>>(mainDraftEntites);
			return ApiResponse<List<RequestMainDraftDto>>.Success(mainDraftDto);
		}

		public async Task<ApiResponse<List<RequestAmendmentDto>>> GetChangeApprovalAsync(string name,
			RequestApproveAmendmentModel requestParametersDto)
		{
			var userId = await _userRepository.ConvertIdentity(name);
			var mainRequest =
				await _requestMainRepository.GetApproveAmendmentRequestsAsync(userId, requestParametersDto);
			var mainRequestDto = _mapper.Map<List<RequestAmendmentDto>>(mainRequest);
			return ApiResponse<List<RequestAmendmentDto>>.Success(mainRequestDto);
		}

		public async Task<ApiResponse<List<RequestApprovalInfoDto>>> GetApprovalInfoAsync(string name,
			int requestMainId)
		{
			var userId = await _userRepository.ConvertIdentity(name);
			var approvalInfo = await _requestMainRepository.GetRequestApprovalInfoAsync(requestMainId, userId);
			var approvalInfoResult = _mapper.Map<List<RequestApprovalInfoDto>>(approvalInfo);

			return ApiResponse<List<RequestApprovalInfoDto>>.Success(approvalInfoResult);
		}

		public async Task<ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>> GetDetails(int requestmainId,
			int businessUnitId)
		{
			var requestDetails =
				await _requestDetailRepository.GetRequestDetailsByMainIdAsync(requestmainId, businessUnitId);
			var requestDetailsResult = _mapper.Map<List<RequestDetailsWithAnalysisCodeDto>>(requestDetails);

			return requestDetailsResult.Count > 0
				? ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Success(requestDetailsResult)
				: ApiResponse<List<RequestDetailsWithAnalysisCodeDto>>.Fail("Request details is empty", 404);
		}

		public async Task<ApiResponse<RequestSaveResultModel>> AddOrUpdateAsync(string name, HttpResponse response,
			RequestSaveModel model)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var resultModel = await _requestMainRepository
				.AddOrUpdateRequestAsync(userId, _mapper.Map<RequestMainSaveModel>(model));

			await _attachmentService.SaveAttachmentAsync(model.Attachments, SourceType.REQ, resultModel.RequestMainId);

			if (resultModel != null)
			{
				var detailIdList = model.Details.Select(x => x.RequestDetailId).ToList();
				var data = detailIdList.Select(x => x == null ? 0 : x).ToList();
				await _requestDetailRepository.DeleteDetailsNotIncludes(data, resultModel.RequestMainId);
				foreach (var detail in model.Details)
				{
					var requestDetailDto = detail;
					if (model.FromStockChanged)
					{
						await SaveRequestDetailsForStockAsync(requestDetailDto);
					}
					else
					{
						requestDetailDto.RequestMainId = resultModel.RequestMainId;
						await SaveRequestDetailsAsync(requestDetailDto);
					}


					if (model.RequestMainId == 0)
						detail.Sequence = 1;

					if (detail.Quantity == 0 && detail.QuantityFromStock > 0)
					{
						var rejectReason = await _generalService.GetRejectReasonByCode("INSTOCK");
						var res = await _requestDetailRepository.RequestDetailChangeStatusAsync(detail.RequestDetailId,
							userId,
							2, null, detail.Sequence, rejectReason.RejectReasonId);
					}
				}

				await _unitOfWork.SaveChangesAsync();

				var detailIds = await _requestMainRepository.GetDetailIds(resultModel.RequestMainId);

				resultModel.RequestDetailIds = detailIds;

				return ApiResponse<RequestSaveResultModel>.Success(resultModel);
			}

			return ApiResponse<RequestSaveResultModel>.Fail("Not Found", 404);
		}

		public async Task<ApiResponse<bool>> DeleteAsync(string name, List<int> requestMainIds)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			foreach (var item in requestMainIds)
			{
				await _requestMainRepository.DeleteAsync(userId, item);
			}

			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(200);
		}

		public async Task<ApiResponse<List<RequestDetailApprovalInfoDto>>> GetDetailApprvalInfoAsync(
			int requestDetaildId)
		{
			var entity = await _requestDetailRepository.GetDetailApprovalInfoAsync(requestDetaildId);
			var result = _mapper.Map<List<RequestDetailApprovalInfoDto>>(entity);

			return result != null
				? ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(result)
				: ApiResponse<List<RequestDetailApprovalInfoDto>>.Success(new List<RequestDetailApprovalInfoDto>());
		}


		public async Task<bool> ChangeDetailStatusAsync(string name, int? requestDetailId, int approveStatusId,
			string comment, int? sequence, int rejectReasonId)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var res = await _requestDetailRepository.RequestDetailChangeStatusAsync(requestDetailId, userId,
				approveStatusId, comment, sequence, rejectReasonId);
			if (res)
				await _unitOfWork.SaveChangesAsync();

			return true;
		}


		public Task<int> DeleteAsync(int userId, int Id)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponse<List<RequestWFADto>>> GetWFAAsync(string name,
			RequestWFAGetModel requestWFAGetParametersDto)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var mainreq = await _requestMainRepository.GetWaitingForApprovalsAsync(userId, requestWFAGetParametersDto);

			var mainRequestDto = _mapper.Map<List<RequestWFADto>>(mainreq);
			if (mainRequestDto != null && mainRequestDto.Count > 0)
				return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto);

			return ApiResponse<List<RequestWFADto>>.Success(mainRequestDto);
		}

		public async Task<ApiResponse<bool>> UpdateBuyerAsync(RequestSetBuyer requestSetBuyer, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			bool data = await _requestMainRepository.UpdateBuyerAsync(requestSetBuyer, userId);
			
			string buyerEmail =
				await _buyerService.FindBuyerEmailByBuyerName(requestSetBuyer.Buyer, requestSetBuyer.BusinessUnitId);
			
			string businessUnitName =
				await _buyerService.FindBuyerEmailByBuyerName(requestSetBuyer.Buyer, requestSetBuyer.BusinessUnitId);
			RequestBuyerData buyerData = new RequestBuyerData();
			buyerData.BuyerName = requestSetBuyer.Buyer;
			buyerData.Email = buyerEmail;
			buyerData.RequestNo = requestSetBuyer.RequestNo;
			buyerData.RequestMainId = 0;
			buyerData.BusinessUnitName = businessUnitName;
			buyerData.Language = "eng";
			
			
			_taskQueue.QueueBackgroundWorkItem(async token =>
			{
				await _mailService.SendRequestBuyerMail(buyerData);
			});

			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(data);
		}

		public async Task<ApiResponse<List<RequestFollowDto>>> GetFollowUsersAsync(int requestMainId)
		{
			var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
			var dto = _mapper.Map<List<RequestFollowDto>>(data);
			if (dto != null && dto.Count > 0)
				return ApiResponse<List<RequestFollowDto>>.Success(dto);
			return ApiResponse<List<RequestFollowDto>>.Fail("Request Follow User List is empty", 400);
		}

		public async Task<ApiResponse<bool>> SaveFollowUserAsync(RequestFollowSaveModel saveModel)
		{
			bool result = await _requestMainRepository.RequestFollowCheckUserExistAsync(saveModel);
			if (!result)
			{
				result = await _requestMainRepository.RequestFollowSaveAsync(saveModel);
				await _unitOfWork.SaveChangesAsync();
				return ApiResponse<bool>.Success(result);
			}

			return ApiResponse<bool>.Fail("This user already exist for this request", 400);
		}

		public async Task<ApiResponse<bool>> DeleteFollowUserAsync(int requestFollowId)
		{
			bool result = false;
			result = await _requestMainRepository.RequestFollowDeleteAsync(requestFollowId);
			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(result);
		}

		private async Task<string[]> GetFollowUserEmailsForRequestAsync(int requestMainId)
		{
			var data = await _requestMainRepository.RequestFollowUserLoadAsync(requestMainId);
			return data?.Select(x => x.Email)?.ToArray();
		}

		public async Task<ApiResponse<int>> GetDefaultApprovalStage(string keyCode, int businessUnitId)
		{
			var data = await _requestMainRepository.GetDefaultApprovalStage(keyCode, businessUnitId);
			return ApiResponse<int>.Success(data);
		}

		public async Task<ApiResponse<List<RequestCategory>>> CategoryList(int businessUnitId, string keyCode)
		{
			var data = await _requestMainRepository.CategoryList(businessUnitId, keyCode);
			if (data.Count > 0)
				return ApiResponse<List<RequestCategory>>.Success(data);
			return ApiResponse<List<RequestCategory>>.Fail("Data not found", 404);
		}

		public async Task<ApiResponse<List<RequestHeldDto>>> GetHeldAsync(RequestWFAGetModel requestMainGet,
			string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var mainreq = await _requestMainRepository.GetHeldAsync(requestMainGet, userId);

			var mainRequestDto = _mapper.Map<List<RequestHeldDto>>(mainreq);
			if (mainRequestDto != null && mainRequestDto.Count > 0)
				return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto);

			return ApiResponse<List<RequestHeldDto>>.Success(mainRequestDto);
		}

		public async Task<ApiResponse<List<BuyersAssignmentDto>>> GetBuyersAssignment(RequestWFAGetModel model,
			string userName)
		{
			int userId = await _userRepository.ConvertIdentity(userName);
			var mainreq = await _requestMainRepository.GetBuyersAssignment(model, userId);

			var mainRequestDto = _mapper.Map<List<BuyersAssignmentDto>>(mainreq);
			if (mainRequestDto != null && mainRequestDto.Count > 0)
				return ApiResponse<List<BuyersAssignmentDto>>.Success(mainRequestDto);

			return ApiResponse<List<BuyersAssignmentDto>>.Success(mainRequestDto);
		}

		public async Task ChangeDetailStatusAndSendMail(string userName, HttpResponse response,
			RequestDetailApproveModel model)
		{
			for (int i = 0; i < model.RequestDetails.Count; i++)
			{
				var res = await ChangeDetailStatusAsync(userName, model.RequestDetails[i].RequestDetailId,
					model.ApproveStatus, model.Comment, model.RequestDetails[i].Sequence, model.RejectReasonId);

			}
		}

		public async Task<ApiResponse<bool>> Retrieve(RequestRetrieveDto data, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			for (int i = 0; i < data.RequestMainIds.Count; i++)
			{
				var res = await _requestMainRepository.Retrieve(data.RequestMainIds[i], userId);
			}
			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(true);
		}

		public async Task<ApiResponse<List<WarehouseInfoDto>>> GetWarehouseList(int businessUnitId)
		{
			var warehouseList = await _requestMainRepository.GetWarehouseList(businessUnitId);
			var dto = _mapper.Map<List<WarehouseInfoDto>>(warehouseList);
			return ApiResponse<List<WarehouseInfoDto>>.Success(dto);
		}

	}
}
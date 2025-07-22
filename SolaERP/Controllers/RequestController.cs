using AngleSharp.Io;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class RequestController : CustomBaseController
	{
		private readonly IRequestService _requestService;
		private readonly IFileUploadService _fileUploadService;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IUserService _userService;
		private readonly IBusinessUnitService _businessUnitService;
		private readonly IGeneralService _generalService;
		private IMailService _mailService;

		public RequestController(IRequestService requestService, IFileUploadService fileUploadService,
			IEmailNotificationService emailNotificationService, IUserService userService,
			IBusinessUnitService businessUnitService, IMailService mailService, IGeneralService generalService)
		{
			_requestService = requestService;
			_fileUploadService = fileUploadService;
			_emailNotificationService = emailNotificationService;
			_userService = userService;
			_businessUnitService = businessUnitService;
			_generalService = generalService;
			_mailService = mailService;
		}

		[HttpGet("{businessUnitId}")]
		public async Task<IActionResult> Type(int businessUnitId)
			=> CreateActionResult(await _requestService.GetTypesAsync(businessUnitId));

		[HttpGet("{requestMainId}")]
		public async Task<IActionResult> MainApprovalInfo(int requestMainId)
			=> CreateActionResult(await _requestService.GetApprovalInfoAsync(User.Identity.Name, requestMainId));

		[HttpGet("{requestDetailId}")]
		public async Task<IActionResult> DetailApprovalInfo(int requestDetailId)
			=> CreateActionResult(await _requestService.GetDetailApprvalInfoAsync(requestDetailId));

		[HttpGet("{requestMainId}")]
		public async Task<IActionResult> FollowUser(int requestMainId)
			=> CreateActionResult(await _requestService.GetFollowUsersAsync(requestMainId));

		[HttpGet]
		public async Task<IActionResult> Info(int requestMainId, int businessUnitId)
			=> CreateActionResult(await _requestService.GetByMainId(User.Identity.Name, requestMainId, businessUnitId));

		[HttpPost]
		public async Task<IActionResult> WaitingForApproval(RequestWFAGetModel requestWFAGetParametersDto)
			=> CreateActionResult(await _requestService.GetWFAAsync(User.Identity.Name, requestWFAGetParametersDto));

		[HttpPost]
		public async Task<IActionResult> All(RequestMainGetModel requestMainParameters)
			=> CreateActionResult(await _requestService.GetAllAsync(requestMainParameters, User.Identity.Name));

		[HttpPost]
		public async Task<IActionResult> Held(RequestWFAGetModel requestMainParameters)
			=> CreateActionResult(await _requestService.GetHeldAsync(requestMainParameters, User.Identity.Name));

		[HttpPost]
		public async Task<IActionResult> ChangeApproval(RequestApproveAmendmentModel requestParametersDto)
			=> CreateActionResult(
				await _requestService.GetChangeApprovalAsync(User.Identity.Name, requestParametersDto));

		[HttpPost]
		public async Task<IActionResult> Draft(RequestMainDraftModel model)
			=> CreateActionResult(await _requestService.GetDraftsAsync(model, User.Identity.Name));

		[HttpPost]
		public async Task<IActionResult> BuyersAssignment(RequestWFAGetModel model)
			=> CreateActionResult(await _requestService.GetBuyersAssignment(model, User.Identity.Name));

		[HttpPost]
		public async Task<IActionResult> Save(RequestSaveModel model)
			=> CreateActionResult(await _requestService.AddOrUpdateAsync(User.Identity.Name, Response, model));

		[HttpPost]
		public async Task<IActionResult> SendToApprove(RequestSendToApproveDto sendToApprove)
		{
			for (int i = 0; i < sendToApprove.RequestMainIds.Count; i++)
			{
				var res = await _requestService.SendToApproveAsync(User.Identity.Name, sendToApprove.RequestMainIds, Response);

			}

			return CreateActionResult(ApiResponse<bool>.Success(200));
		}


		[HttpPost]
		public async Task<IActionResult> Retrieve(RequestRetrieveDto data)
		{
			var res = await _requestService.Retrieve(data, User.Identity.Name);
			return CreateActionResult(ApiResponse<bool>.Success(200));
		}

		[HttpPost]
		public async Task<IActionResult> ChangeMainStatus(RequestChangeStatusModel data)
		{
			var rejectReasonCode = await _generalService.ReasonCode(data.RejectReasonId);
			if (rejectReasonCode == "STOPPED")
			{
				data.ApproveStatus = 4;
			}
			for (int i = 0; i < data.RequestDatas.Count; i++)
			{
				var res = await _requestService.ChangeMainStatusAsync(User.Identity.Name,
					data.RequestDatas[i].RequestMainId, data.ApproveStatus, data.Comment, data.RejectReasonId, data.BusinessUnitName, data.RequestDatas[i].Sequence, Response);

			}

			return CreateActionResult(ApiResponse<bool>.Success(200));
		}

		[HttpPost]
		public async Task<IActionResult> ChangeDetailStatus(RequestDetailApproveModel model)
		{
			for (int i = 0; i < model.RequestDetails.Count; i++)
			{
				var res = await _requestService.ChangeDetailStatusAsync(User.Identity.Name,
					model.RequestDetails[i].RequestDetailId, model.ApproveStatus, model.Comment,
					model.RequestDetails[i].Sequence, model.RejectReasonId);
			}
			return CreateActionResult(ApiResponse<bool>.Success(200));
		}

		[HttpPost]
		public async Task<IActionResult> UpdateBuyer(RequestSetBuyer requestSetBuyer)
			=> CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer, User.Identity.Name));


		[HttpPost]
		public async Task<IActionResult> CreateFollowUser(RequestFollowSaveModel saveModel)
			=> CreateActionResult(await _requestService.SaveFollowUserAsync(saveModel));

		[HttpDelete]
		public async Task<IActionResult> Delete(List<int> requestMainIds)
			=> CreateActionResult(await _requestService.DeleteAsync(User.Identity.Name, requestMainIds));

		[HttpDelete("{requestFollowId}")]
		public async Task<IActionResult> DeleteFollowUser(int requestFollowId)
			=> CreateActionResult(await _requestService.DeleteFollowUserAsync(requestFollowId));

		[HttpGet("{keyCode}/{businessUnitId}")]
		public async Task<IActionResult>
			GetDefaultApprovalStage(string keyCode, int businessUnitId) //return data by Request Type(SR/LR/PR/RR)
			=> CreateActionResult(await _requestService.GetDefaultApprovalStage(keyCode, businessUnitId));

		[HttpGet]
		public async Task<IActionResult> CategoryList(int businessUnitId, string keyCode)
			=> CreateActionResult(await _requestService.CategoryList(businessUnitId, keyCode));
		
		[HttpGet]
		public async Task<IActionResult> GetWarehouseList(int businessUnitId)
			=> CreateActionResult(await _requestService.GetWarehouseList(businessUnitId));
		
	}
}
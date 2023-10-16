using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IMailService _mailService;
        public RequestController(IRequestService requestService, IFileUploadService fileUploadService, IEmailNotificationService emailNotificationService, IUserService userService, IMailService mailService)
        {
            _requestService = requestService;
            _fileUploadService = fileUploadService;
            _emailNotificationService = emailNotificationService;
            _userService = userService;
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
        public async Task<IActionResult> Info(int requestMainId)
           => CreateActionResult(await _requestService.GetByMainId(User.Identity.Name, requestMainId));

        [HttpPost]
        public async Task<IActionResult> WaitingForApproval(RequestWFAGetModel requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWFAAsync(User.Identity.Name, requestWFAGetParametersDto));

        [HttpPost]
        public async Task<IActionResult> All(RequestMainGetModel requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> Held(RequestWFAGetModel requestMainParameters)
         => CreateActionResult(await _requestService.GetHeldAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> ChangeApproval(RequestApproveAmendmentModel requestParametersDto)
            => CreateActionResult(await _requestService.GetChangeApprovalAsync(User.Identity.Name, requestParametersDto));

        [HttpPost]
        public async Task<IActionResult> Draft(RequestMainDraftModel model)
            => CreateActionResult(await _requestService.GetDraftsAsync(model));

        [HttpPost]
        public async Task<IActionResult> Save(RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> SendToApprove(RequestSendToApproveDto sendToApprove)
        => CreateActionResult(await _requestService.SendToApproveAsync(User.Identity.Name, sendToApprove.RequestMainIds));

        [HttpPost]
        public async Task<IActionResult> ChangeMainStatus(RequestChangeStatusModel data)
        {
            var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQP);

            for (int i = 0; i < data.RequestDatas.Count; i++)
            {
                var res = await _requestService.ChangeMainStatusAsync(User.Identity.Name, data.RequestDatas[i].RequestMainId, data.ApproveStatus, data.Comment, data.RejectReasonId);

                if (res)
                {
                    var users = await _userService.UsersForRequestMain(data.RequestDatas[i].RequestMainId, data.RequestDatas[i].Sequence, (ApproveStatus)data.ApproveStatus);
                    await _mailService.SendRequestMailsForChangeStatus(Response, users, data.RequestDatas[i].Sequence, data.BusinessUnitName, data.RejectReason);
                }
            }

            return CreateActionResult(ApiResponse<bool>.Success(200));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDetailStatus(RequestDetailApproveModel model)
        {
            for (int i = 0; i < model.RequestDetailIds.Count; i++)
            {
                var res = await _requestService.ChangeDetailStatusAsync(User.Identity.Name, model.RequestDetailIds[i], model.ApproveStatus, model.Comment, model.Sequence, model.RejectReasonId);
                if (res)
                {
                    var users = await _userService.UsersRequestDetails(model.RequestDetailIds[0], model.Sequence, (ApproveStatus)model.ApproveStatus);
                    await _mailService.SendRequestMailsForChangeStatus(Response, users, model.Sequence, model.BusinessUnitName, model.RejectReason);
                }
            }

            return CreateActionResult(ApiResponse<bool>.Success(200));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBuyer(List<RequestSetBuyer> requestSetBuyer)
            => CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer));

        [HttpPost]
        public async Task<IActionResult> CreateFollowUser(RequestFollowSaveModel saveModel)
            => CreateActionResult(await _requestService.SaveFollowUserAsync(saveModel));

        [HttpDelete("{requestMainId}")]
        public async Task<IActionResult> Delete(int requestMainId)
            => CreateActionResult(await _requestService.DeleteAsync(User.Identity.Name, requestMainId));

        [HttpDelete("{requestFollowId}")]
        public async Task<IActionResult> DeleteFollowUser(int requestFollowId)
            => CreateActionResult(await _requestService.DeleteFollowUserAsync(requestFollowId));

        [HttpGet("{keyCode}/{businessUnitId}")]
        public async Task<IActionResult> GetDefaultApprovalStage(string keyCode, int businessUnitId) //return data by Request Type(SR/LR/PR/RR)
            => CreateActionResult(await _requestService.GetDefaultApprovalStage(keyCode, businessUnitId));

        [HttpGet]
        public async Task<IActionResult> CategoryList()
            => CreateActionResult(await _requestService.CategoryList());


    }
}


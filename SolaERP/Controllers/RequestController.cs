using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SolaERP.API.Extensions;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.ViewModels;

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
        public async Task<IActionResult> ChangeMainStatus(RequestChangeStatusModel requestChangeStatusParametersDto)
        {
            var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQP);

            for (int i = 0; i < requestChangeStatusParametersDto.RequestMainIds.Count; i++)
            {
                var res = await _requestService.ChangeMainStatusAsync(User.Identity.Name, requestChangeStatusParametersDto.RequestMainIds[i], requestChangeStatusParametersDto.ApproveStatus, requestChangeStatusParametersDto.Comment);

                if (res)
                {
                    foreach (var lang in Enum.GetValues<Language>())
                    {
                        var sendUsersMails = await _userService.GetAdminUserMailsAsync(1, lang);
                        if (sendUsersMails.Count > 0)
                        {
                            var temp = templates.First(x => x.Language == lang.ToString());
                            VM_RequestPending requestPending = new VM_RequestPending
                            {
                                Body = new HtmlString(temp.Body),
                                Sequence = 1,
                                FullName = "hulya",
                                Header = temp.Header,
                                Subject = temp.Subject,
                            };

                            string body = await GetMailBody<VM_RequestPending>.GetBody(requestPending, @"RequestPending.cshtml");
                            MailModel mailModel = new MailModel()
                            {
                                Body = body,
                                Header = requestPending.Header,
                                EmailType = EmailTemplateKey.REQP,
                                Subject = requestPending.Subject,
                                Tos = new List<string> { "hulya.garibli@apertech.met" }
                            };
                            await _mailService.SendRequest(mailModel);
                        }
                    }
                }
            }

            return CreateActionResult(ApiResponse<bool>.Success(200));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDetailStatus(RequestDetailApproveModel model)
            => CreateActionResult(await _requestService.ChangeDetailStatusAsync(User.Identity.Name, model));

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

        //[HttpPost]
        //public async Task<string> Test([FromForm] UploadFile uploadFile)
        //{
        //    await _fileUploadService.UploadFile(uploadFile);
        //    return null;
        //}
    }
}


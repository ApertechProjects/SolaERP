using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
    [ApiController]
    [Authorize]
    public class RequestController : CustomBaseController
    {
        private readonly IRequestService _requestService;
        private IUserRepository _userRepository;

        public RequestController(IRequestService requestService, IUserRepository userRepository)
        {
            _requestService = requestService;
            _userRepository = userRepository;
        }

        [HttpGet("api/[controller]/[action]/{businessUnitId}")]
        public async Task<IActionResult> Type(int businessUnitId)
            => CreateActionResult(await _requestService.GetRequestTypesAsync(businessUnitId));

        [HttpGet("api/[controller]/[action]/{requestMainId}")]
        public async Task<IActionResult> ApprovalInfo(int requestMainId)
            => CreateActionResult(await _requestService.GetApprovalInfoAsync(User.Identity.Name, requestMainId));

        [HttpGet("api/[controller]/[action]/{requestDetailId}")]
        public async Task<IActionResult> DetailApprovalInfo(int requestDetailId)
            => CreateActionResult(await _requestService.GetRequestDetailApprvalInfoAsync(requestDetailId));

        [HttpGet("api/[controller]/[action]/{requestMainId}")]
        public async Task<IActionResult> FollowUsers(int requestMainId)
            => CreateActionResult(await _requestService.RequestFollowUserLoadAsync(requestMainId));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> WaitingForApproval(RequestWFAGetModel requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(User.Identity.Name, requestWFAGetParametersDto));

        [HttpPost]
        public async Task<IActionResult> All(RequestMainGetModel requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> ChangeApproval(RequestApproveAmendmentModel requestParametersDto)
            => CreateActionResult(await _requestService.GetChangeApprovalAsync(User.Identity.Name, requestParametersDto));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> Drafts(RequestMainDraftModel model)
            => CreateActionResult(await _requestService.GetDraftsAsync(model));

        [HttpGet("api/[controller]/[action]/{requestMainId}")]
        public async Task<IActionResult> Info(int requestMainId)
             => CreateActionResult(await _requestService.GetByMainId(User.Identity.Name, requestMainId));

        [HttpPost]
        public async Task<IActionResult> Save(RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateRequestAsync(User.Identity.Name, model));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> SendToApprove(int requestMainId)
        => CreateActionResult(await _requestService.SendToApproveAsync(User.Identity.Name, requestMainId));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> ChangeMainStatus(RequestChangeStatusModel requestChangeStatusParametersDto)
            => CreateActionResult(await _requestService.ChangeMainStatusAsync(User.Identity.Name, requestChangeStatusParametersDto));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> ChangeDetailStatus(RequestDetailApproveModel model)
            => CreateActionResult(await _requestService.RequestDetailChangeStatusAsync(User.Identity.Name, model));

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> UpdateBuyer(List<RequestSetBuyer> requestSetBuyer)
            => CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer));


        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> SaveFollowUser(RequestFollowSaveModel saveModel)
            => CreateActionResult(await _requestService.RequestFollowSaveAsync(saveModel));

        [HttpDelete("{requestMainId}")]
        public async Task<IActionResult> Delete(int requestMainId)
            => CreateActionResult(await _requestService.DeleteRequestAsync(User.Identity.Name, requestMainId));


        [HttpDelete("api/[controller]/[action]/{requestFollowId}")]
        public async Task<IActionResult> DeleteRequestFollow(int requestFollowId)
            => CreateActionResult(await _requestService.RequestFollowDeleteAsync(requestFollowId));
    }
}


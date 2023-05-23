using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
            => CreateActionResult(await _requestService.GetRequestTypesByBusinessUnitIdAsync(businessUnitId));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> GetRequestApprovalInfo(int requestMainId)
            => CreateActionResult(await _requestService.GetRequestApprovalInfoAsync(User.Identity.Name, requestMainId));

        [HttpGet("{requestDetailId}")]
        public async Task<IActionResult> GetDetailApprovalInfoAsync(int requestDetailId)
            => CreateActionResult(await _requestService.GetRequestDetailApprvalInfoAsync(requestDetailId));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> RequestFollowUsersLoadAsync(int requestMainId)
            => CreateActionResult(await _requestService.RequestFollowUserLoadAsync(requestMainId));

        [HttpGet]
        public async Task<IActionResult> GetWaitingForApprovalsRequest([FromQuery] RequestWFAGetModel requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(User.Identity.Name, requestWFAGetParametersDto));

        [HttpGet]
        public async Task<IActionResult> GetAllMainRequestAsync([FromQuery] RequestMainGetModel requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequestsAsync(RequestApproveAmendmentModel requestParametersDto)
            => CreateActionResult(await _requestService.GetApproveAmendmentRequests(User.Identity.Name, requestParametersDto));


        [HttpPost]
        public async Task<IActionResult> GetRequestDraftsAsync(RequestMainDraftModel model)
            => CreateActionResult(await _requestService.GetRequestMainDraftsAsync(model));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> GetRequestCardByMainId(int requestMainId)
             => CreateActionResult(await _requestService.GetRequestByRequestMainId(User.Identity.Name, requestMainId));


        [HttpPost]
        public async Task<IActionResult> SaveRequestAsync(RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateRequestAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> RequestSendToApproveAsync(int requestMainId)
        => CreateActionResult(await _requestService.RequestSendToApproveAsync(User.Identity.Name, requestMainId));

        [HttpPost]
        public async Task<IActionResult> RequestMainChangeStatusAsync(RequestChangeStatusModel requestChangeStatusParametersDto)
            => CreateActionResult(await _requestService.RequestMainChangeStatusAsync(User.Identity.Name, requestChangeStatusParametersDto));

        [HttpPost]
        public async Task<IActionResult> RequestDetailChangeStatusAsync(RequestDetailApproveModel model)
            => CreateActionResult(await _requestService.RequestDetailChangeStatusAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> UpdateBuyerAsync(List<RequestSetBuyer> requestSetBuyer)
            => CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer));


        [HttpPost]
        public async Task<IActionResult> RequestFollowSaveAsync(RequestFollowSaveModel saveModel)
            => CreateActionResult(await _requestService.RequestFollowSaveAsync(saveModel));

        [HttpDelete("{requestMainId}")]
        public async Task<IActionResult> DeleteRequest(int requestMainId)
            => CreateActionResult(await _requestService.DeleteRequestAsync(User.Identity.Name, requestMainId));


        [HttpDelete("{requestFollowId}")]
        public async Task<IActionResult> RequestFollowDeleteAsync(int requestFollowId)
            => CreateActionResult(await _requestService.RequestFollowDeleteAsync(requestFollowId));
    }
}


using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> GetRequestApprovalInfo([FromHeader] string authToken, int requestMainId)
            => CreateActionResult(await _requestService.GetRequestApprovalInfoAsync(authToken, requestMainId));

        [HttpGet("{requestDetailId}")]
        public async Task<IActionResult> GetDetailApprovalInfoAsync(int requestDetailId)
            => CreateActionResult(await _requestService.GetRequestDetailApprvalInfoAsync(requestDetailId));

        [HttpGet]
        public async Task<IActionResult> RequestFollowUsersLoadAsync(int requestMainId)
            => CreateActionResult(await _requestService.RequestFollowUserLoadAsync(requestMainId));

        [HttpPost]
        public async Task<IActionResult> GetWaitingForApprovalsRequest([FromHeader] string authToken, RequestWFAGetModel requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(authToken, requestWFAGetParametersDto));

        [HttpPost]
        public async Task<IActionResult> GetAllMainRequestAsync(RequestMainGetModel requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequestsAsync([FromHeader] string authToken, RequestApproveAmendmentModel requestParametersDto)
            => CreateActionResult(await _requestService.GetApproveAmendmentRequests(authToken, requestParametersDto));


        [HttpPost]
        public async Task<IActionResult> GetRequestDraftsAsync(RequestMainDraftModel model)
            => CreateActionResult(await _requestService.GetRequestMainDraftsAsync(model));

        [HttpPost]
        public async Task<IActionResult> GetRequestCardByMainId([FromHeader] string authToken, int requestMainId)
             => CreateActionResult(await _requestService.GetRequestByRequestMainId(authToken, requestMainId));


        [HttpPost]
        public async Task<IActionResult> SaveRequestAsync([FromHeader] string authToken, RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateRequestAsync(authToken, model));

        [HttpPost]
        public async Task<IActionResult> RequestSendToApproveAsync([FromHeader] string authToken, int requestMainId)
        => CreateActionResult(await _requestService.RequestSendToApproveAsync(authToken, requestMainId));

        [HttpPost]
        public async Task<IActionResult> RequestMainChangeStatusAsync([FromHeader] string authToken, RequestChangeStatusModel requestChangeStatusParametersDto)
            => CreateActionResult(await _requestService.RequestMainChangeStatusAsync(authToken, requestChangeStatusParametersDto));

        [HttpPost]
        public async Task<IActionResult> RequestDetailChangeStatusAsync([FromHeader] string authToken, RequestDetailApproveModel model)
            => CreateActionResult(await _requestService.RequestDetailChangeStatusAsync(authToken, model));

        [HttpPost]
        public async Task<IActionResult> UpdateBuyerAsync(RequestSetBuyer requestSetBuyer)
            => CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer));


        [HttpPost]
        public async Task<IActionResult> RequestFollowSaveAsync([FromHeader] string authToken, RequestFollowSaveModel saveModel)
            => CreateActionResult(await _requestService.RequestFollowSaveAsync(authToken, saveModel));

        [HttpDelete("{requestMainId}")]
        public async Task<IActionResult> DeleteRequest([FromHeader] string authToken, int requestMainId)
            => CreateActionResult(await _requestService.DeleteRequestAsync(authToken, requestMainId));


        [HttpDelete("{requestFollowId}")]
        public async Task<IActionResult> RequestFollowDeleteAsync(int requestFollowId)
            => CreateActionResult(await _requestService.RequestFollowDeleteAsync(requestFollowId));
    }
}


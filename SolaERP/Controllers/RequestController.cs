using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RequestController : CustomBaseController
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllMainRequestAsync(RequestMainGetParametersDto requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> SaveRequestAsync([FromHeader] string authToken, RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateAsync(authToken, model));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
            => CreateActionResult(await _requestService.GetRequestTypesByBusinessUnitIdAsync(businessUnitId));

        [HttpPost]
        public async Task<IActionResult> ChangeRequestStatusAsync([FromHeader] string authToken, List<RequestChangeStatusModel> requestChangeStatusParametersDtos)
            => CreateActionResult(await _requestService.ChangeRequestStatus(authToken, requestChangeStatusParametersDtos));

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequestsAsync([FromHeader] string authToken, RequestApproveAmendmentGetModel requestParametersDto)
            => CreateActionResult(await _requestService.GetApproveAmendmentRequests(authToken, requestParametersDto));

        [HttpPost]
        public async Task<IActionResult> GetWaitingForApprovalsRequest([FromHeader] string authToken, RequestWFAGetParametersDto requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(authToken, requestWFAGetParametersDto));

        [HttpPost]
        public async Task<IActionResult> GetRequestCardByMainId([FromHeader] string authToken, int requestMainId)
             => CreateActionResult(await _requestService.GetRequestByRequestMainId(authToken, requestMainId));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> GetRequestApprovalInfo([FromHeader] string authToken, int requestMainId)
            => CreateActionResult(await _requestService.GetRequestApprovalInfoAsync(authToken, requestMainId));



    }
}


using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;

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
        public async Task<IActionResult> SaveRequestAsync(RequestMainWithDetailsDto requestMainDto)
        => CreateActionResult(await _requestService.AddOrUpdateAsync(requestMainDto));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
            => CreateActionResult(await _requestService.GetRequestTypesByBusinessUnitIdAsync(businessUnitId));

        [HttpPost]
        public async Task<IActionResult> ChangeRequestStatusAsync(List<RequestChangeStatusParametersDto> requestChangeStatusParametersDtos)
            => CreateActionResult(await _requestService.ChangeRequestStatus(requestChangeStatusParametersDtos));

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequestsAsync(RequestApproveAmendmentGetParametersDto requestParametersDto)
            => CreateActionResult(await _requestService.GetApproveAmendmentRequests(requestParametersDto));

        [HttpGet]
        public async Task<IActionResult> GetWaitingForApprovalsRequest(RequestWFAGetParametersDto requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(requestWFAGetParametersDto));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> GetRequestByRequestMainId(int requestMainId)
             => CreateActionResult(await _requestService.GetRequestByRequestMainId(requestMainId));
    }
}


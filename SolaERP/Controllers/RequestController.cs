using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;

namespace SolaERP.Controllers
{
    [Route("/[action]")]
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
        public async Task<IActionResult> SaveRequestAsync(RequestMainDto requestMainDto)
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

        [HttpPost]
        public async Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM)
        {
            return await _requestService.SaveRequest(requestSaveVM);
        }

        [HttpPost]
        public async Task<IActionResult> GetWaitingForApprovalsRequest(RequestWFAGetParametersDto requestWFAGetParametersDto)
        {
            return CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(requestWFAGetParametersDto));
        }

        [HttpPost]
        public async Task<IActionResult> GetRequestByRequestMainId(int requestMainId)
        {
            return CreateActionResult(await _requestService.GetRequestByRequestMainId(requestMainId));
        }
    }
}


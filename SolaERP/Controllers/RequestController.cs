using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.ViewModels;

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
        public async Task<ApiResponse<List<RequestMainDto>>> GetAllMainRequest(RequestMainGetParametersDto requestMainParameters)
        {
            return await _requestService.GetAllAsync(requestMainParameters);
        }

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
    }
}

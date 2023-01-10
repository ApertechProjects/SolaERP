using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RequestController : ControllerBase
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
        public async Task<ApiResponse<RequestMainDto>> SaveRequest(RequestMainDto requestMainDto)
        {
            return await _requestService.AddOrUpdateAsync(requestMainDto);
        }

        [HttpGet("{businessUnitId}")]
        public async Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId)
        {
            return await _requestService.GetRequestTypesByBusinessUnitId(businessUnitId);
        }
    }
}

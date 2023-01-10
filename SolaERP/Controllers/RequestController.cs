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
        public async Task<ApiResponse<RequestSaveVM>> SaveRequest(RequestSaveVM requestSaveVM)
        {
            return await _requestService.SaveRequest(requestSaveVM);
        }

        [HttpGet("{businessUnitId}")]
        public async Task<List<RequestTypesDto>> GetRequestTypesByBusinessUnitId(int businessUnitId)
        {
            return await _requestService.GetRequestTypesByBusinessUnitId(businessUnitId);
        }
    }
}

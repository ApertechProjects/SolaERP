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
        public async Task<IActionResult> ChangeRequestStatus(List<RequestChangeStatusParametersDto> requestChangeStatusParametersDtos)
            => CreateActionResult(await _requestService.ChangeRequestStatus(requestChangeStatusParametersDtos));

        [HttpPost]
        public async Task<ApiResponse<bool>> ChangeRequestStatus(List<RequestChangeStatusParametersDto> requestChangeStatusParametersDtos)
        {
            return await _requestService.ChangeRequestStatus(requestChangeStatusParametersDtos);
        }

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequests(RequestApproveAmendmentGetParametersDto requestParametersDto)
        {
            return CreateActionResult(await _requestService.GetApproveAmendmentRequests(requestParametersDto));
        }

        public async Task<IActionResult> GetAllMainDraftsAsync(RequestMainDraftGetDto mainDraftRequest)
            => CreateActionResult(await _requestService.GetAllRequestMainDraftsAsync(mainDraftRequest));
    }
}


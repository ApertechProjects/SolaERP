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
        //private ICommonService<RequestWFADto> _commonServiceForWFA;
        private IUserRepository _userRepository;

        public RequestController(IRequestService requestService, IUserRepository userRepository)
        {
            _requestService = requestService;
            //_commonServiceForWFA = commonServiceForWFA;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllMainRequestAsync(RequestMainGetModel requestMainParameters)
            => CreateActionResult(await _requestService.GetAllAsync(requestMainParameters));

        [HttpPost]
        public async Task<IActionResult> SaveRequestAsync([FromHeader] string authToken, RequestSaveModel model)
        => CreateActionResult(await _requestService.AddOrUpdateRequestAsync(authToken, model));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
            => CreateActionResult(await _requestService.GetRequestTypesByBusinessUnitIdAsync(businessUnitId));

        [HttpPost]
        public async Task<IActionResult> ChangeRequestStatusAsync([FromHeader] string authToken, List<RequestChangeStatusModel> requestChangeStatusParametersDtos)
            => CreateActionResult(await _requestService.ChangeRequestStatus(authToken, requestChangeStatusParametersDtos));

        [HttpPost]
        public async Task<IActionResult> GetApproveAmendmentRequestsAsync([FromHeader] string authToken, RequestApproveAmendmentModel requestParametersDto)
            => CreateActionResult(await _requestService.GetApproveAmendmentRequests(authToken, requestParametersDto));

        [HttpPost]
        public async Task<IActionResult> GetWaitingForApprovalsRequest([FromHeader] string authToken, RequestWFAGetModel requestWFAGetParametersDto)
            => CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(authToken, requestWFAGetParametersDto));

        [HttpPost]
        public async Task<IActionResult> GetRequestCardByMainId([FromHeader] string authToken, int requestMainId)
             => CreateActionResult(await _requestService.GetRequestByRequestMainId(authToken, requestMainId));

        [HttpGet("{requestMainId}")]
        public async Task<IActionResult> GetRequestApprovalInfo([FromHeader] string authToken, int requestMainId)
            => CreateActionResult(await _requestService.GetRequestApprovalInfoAsync(authToken, requestMainId));

        [HttpPost]
        public async Task<IActionResult> GetRequestDraftsAsync(RequestMainDraftModel model)
            => CreateActionResult(await _requestService.GetRequestMainDraftsAsync(model));

        [HttpDelete("{requestMainId}")]
        public async Task<IActionResult> DeleteRequest([FromHeader] string authToken, int requestMainId)
            => CreateActionResult(await _requestService.DeleteRequestAsync(authToken, requestMainId));

        [HttpGet("{reqeustDetailId}")]
        public async Task<IActionResult> GetDetailApprovalInfoAsync(int reqeustDetailId)
            => CreateActionResult(await _requestService.GetRequestDetailApprvalInfoAsync(reqeustDetailId));

        [HttpPost]
        public async Task<IActionResult> SendDetailToApprove([FromHeader] string authToken, RequestDetailSendToApproveModel model)
            => CreateActionResult(await _requestService.RequestDetailSendToApprove(authToken, model));

        [HttpPost]
        public async Task<IActionResult> UpdateBuyerAsync(RequestSetBuyer requestSetBuyer)
            => CreateActionResult(await _requestService.UpdateBuyerAsync(requestSetBuyer));

    }
}


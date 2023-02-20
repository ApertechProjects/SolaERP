using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RequestController : CustomBaseController
    {
        private readonly IRequestService _requestService;
        private ICommonService<RequestWFADto> _commonServiceForWFA;
        private IUserRepository _userRepository;

        public RequestController(IRequestService requestService, ICommonService<RequestWFADto> commonServiceForWFA, IUserRepository userRepository)
        {
            _requestService = requestService;
            _commonServiceForWFA = commonServiceForWFA;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllMainRequestAsync(RequestMainGetParametersDto requestMainParameters)
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
        public async Task<IActionResult> GetWaitingForApprovalsRequest([FromHeader] string authToken, RequestWFAGetParametersDto requestWFAGetParametersDto)
        {
            List<ExecuteQueryParamList> paramListReplace = new List<ExecuteQueryParamList>();
            paramListReplace.Add(new ExecuteQueryParamList { ParamName = "@ItemCode", Value = "'01070201001','01070201002'" });

            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);

            List<ExecuteQueryParamList> paramListOrdinary = new List<ExecuteQueryParamList>();
            paramListOrdinary.Add(new ExecuteQueryParamList { ParamName = "@BusinessUnitId", Value = requestWFAGetParametersDto.BusinessUnitId });
            paramListOrdinary.Add(new ExecuteQueryParamList { ParamName = "@DateFrom", Value = requestWFAGetParametersDto.DateFrom });
            paramListOrdinary.Add(new ExecuteQueryParamList { ParamName = "@DateTo", Value = requestWFAGetParametersDto.DateTo });
            paramListOrdinary.Add(new ExecuteQueryParamList { ParamName = "@UserId", Value = userId });

            return CreateActionResult(await _commonServiceForWFA.ExecQueryWithReplace("[dbo].[SP_RequestMainWFA1]", paramListReplace, paramListOrdinary));
            //return CreateActionResult(await _requestService.GetWaitingForApprovalsAsync(authToken, requestWFAGetParametersDto));
        }

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

    }
}


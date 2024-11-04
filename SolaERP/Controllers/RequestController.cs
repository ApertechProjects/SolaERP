using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RequestController : CustomBaseController
    {
        private readonly IRequestService _requestService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUserService _userService;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IGeneralService _generalService;
        private IMailService _mailService;

        public RequestController(IRequestService requestService, IFileUploadService fileUploadService,
            IEmailNotificationService emailNotificationService, IUserService userService,
            IBusinessUnitService businessUnitService, IMailService mailService, IGeneralService generalService)
        {
            _requestService = requestService;
            _fileUploadService = fileUploadService;
            _emailNotificationService = emailNotificationService;
            _userService = userService;
            _businessUnitService = businessUnitService;
            _generalService = generalService;
            _mailService = mailService;
        }

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> Type(int businessUnitId)
            => CreateActionResult(await _requestService.GetTypesAsync(businessUnitId));

        [HttpGet]
        public async Task<IActionResult> Info(int requestMainId, int businessUnitId)
            => CreateActionResult(await _requestService.GetByMainId(User.Identity.Name, requestMainId, businessUnitId));

    }
}
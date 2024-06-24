using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ReportController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IUserReportService _userReportService;
        public ReportController(IUserService userService, IUserReportService userReportService)
        {
            _userService = userService;
            _userReportService = userReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReportAccess(string reportFileId)
         => CreateActionResult(await _userService.GetUserReportAccess(reportFileId));

        [HttpGet]
        public async Task<IActionResult> GetUserReportAccessByCurrentUser()
         => CreateActionResult(await _userService.GetUserReportAccessByCurrentUser(User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> SaveUserReportAccess(UserReportSaveDto data)
         => CreateActionResult(await _userReportService.Save(data));
    }
}

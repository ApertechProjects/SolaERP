using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEnvController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TestEnvController(IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> SenTestTemplateEmail()
        {
            //string templatePath = _webHostEnvironment.WebRootPath;

            return Ok(await _mailService.SendEmailMessage("C:\\Users\\HP\\source\\repos\\solaerp\\SolaERP\\wwwroot\\sources\\templates\\RegistrationIsPendingForApprovals.html", "yaqub.nasibov@apertech.net", "Test From Env"));
        }

    }
}

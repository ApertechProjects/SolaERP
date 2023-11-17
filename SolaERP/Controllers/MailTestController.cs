using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailTestController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailTestController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<string> SendMail(string to)
        {
            try
            {
                await _mailService.SendSafeMailsAsync(new string[] { to }, "Test", "Test");
                return "Operation Successful";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
    }
}

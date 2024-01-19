using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MailTestController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailTestController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<string> SendMail()
        {
            try
            {
                await _mailService.SendManualMailsAsync("hulya.garibli@apertech.net");
                return "Operation Successful";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

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
                await _mailService.SendManualMailsAsync(to);
                return "Operation Successful";
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }
        }
    }
}

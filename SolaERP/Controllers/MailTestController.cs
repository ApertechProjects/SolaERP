using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
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
        public async Task<string> SendMail([FromQuery] MailProperty mailProperty)
        {
            try
            {
                await _mailService.SendManualMailsAsync(mailProperty.Email, mailProperty.Password, mailProperty.Host, mailProperty.Port, mailProperty.To);
                return "Operation Successful";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

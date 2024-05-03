using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Enums;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailTestController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IKafkaMailService _kafkaMailService;
        public MailTestController(IMailService mailService, IKafkaMailService kafkaMailService)
        {
            _mailService = mailService;
            _kafkaMailService = kafkaMailService;
        }

        [HttpPost]
        public async Task SendMail()
        {
            List<Person> persons = new List<Person>()
            {
                new Person{Email = "hulya.garibli@apertech.net",UserName = "Hulya Garibli"}
            };

            await _kafkaMailService.SendMail(EmailTemplate.RequestApproved, ApproveStatus.Approved, persons,1,"PR-2343-92342");
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

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
        private readonly ISupplierEvaluationService _supplierEvaluationService;


        public TestEnvController(IMailService mailService, IWebHostEnvironment webHostEnvironment, ISupplierEvaluationService supplierEvaluationService)
        {
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
            _supplierEvaluationService = supplierEvaluationService;
        }

        public override bool Equals(object obj)
        {
            return obj is TestEnvController controller &&
                   EqualityComparer<ISupplierEvaluationService>.Default.Equals(_supplierEvaluationService, controller._supplierEvaluationService);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_supplierEvaluationService);
        }

        //[HttpGet]
        //public async Task<IActionResult> SenTestTemplateEmail()
        //{
        //    //string templatePath = _webHostEnvironment.WebRootPath;

        //    return Ok(await _mailService.SendEmailMessage("C:\\Users\\HP\\source\\repos\\solaerp\\SolaERP\\wwwroot\\sources\\templates\\RegistrationIsPendingForApprovals.html", "yaqub.nasibov@apertech.net", "Test From Env"));
        //}



        //[HttpGet]
        //public async Task<IActionResult> TestDueDiligence()
        //{

        //    //return Ok(await _supplierEvaluationService.GetAllAsync());
        //}

    }
}

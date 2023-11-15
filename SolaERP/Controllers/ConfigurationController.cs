using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Helper;
using System.Text.Json;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        //private readonly IConfiguration _configuration;

        public ConfigurationController()
        {
            //_configuration = configuration;
        }

        [HttpGet]
        public string GetConfiguration()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: true);

            IConfiguration _configuration = builder.Build();

            return _configuration["Mail:Url"];
        }
    }
}

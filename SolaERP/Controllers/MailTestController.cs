using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Enums;
using SolaERP.Infrastructure.ViewModels;
using System.Web;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailTestController : ControllerBase
    {

    }
}

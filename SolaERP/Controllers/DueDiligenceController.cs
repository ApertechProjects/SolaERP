using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;
using SolaERP.Business.CommonLogic;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DueDiligenceController : ControllerBase
    {
        public DueDiligenceController(ConfHelper confHelper)
        {
            conf = confHelper;
        }

        public ConfHelper conf { get; }

        [HttpGet("{vendorId}")]
        public async Task<ApiResult> GetDue([FromHeader] string token,int vendorId)
        {
            return await new EntityLogic(conf).GetDueDiligence(token,vendorId);
        }
    }
}

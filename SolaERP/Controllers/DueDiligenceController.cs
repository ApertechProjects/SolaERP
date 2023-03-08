using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DueDiligenceController : ControllerBase
    {
        public DueDiligenceController(ConfHelper confHelper)
        {
            conf = confHelper;
        }

        public ConfHelper conf { get; }

        [HttpGet("{vendorId}")]
        public async Task<ApiResult> GetDue([FromHeader] string token, int vendorId)
        {
            return await new EntityLogic(conf).GetDueDiligence(token, vendorId);
        }
    }
}

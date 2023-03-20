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

        /// <summary>
        ///Retrieves all due diligence tasks for a given vendor.
        /// </summary>
        /// <remarks>The GetDue endpoint retrieves a list of all due diligence tasks for a given vendor, identified by the vendorId parameter.</remarks>
        ///<param name="token">The token used to authenticate the user who performs the operation</param>
        [HttpGet("{vendorId}")]
        public async Task<ApiResult> GetDue([FromHeader] string token, int vendorId)
        {
            return await new EntityLogic(conf).GetDueDiligence(token, vendorId);
        }
    }
}

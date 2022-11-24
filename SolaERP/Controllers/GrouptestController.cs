using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Dtos.EntityDtos.Group;
using SolaERP.Business.Dtos.Wrappers;
using SolaERP.Business.Models;


namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GrouptestController : ControllerBase
    {

        public GrouptestController(ConfHelper confHelper)
        {
            ConfHelper = confHelper;
        }

        public ConfHelper ConfHelper { get; }

        [HttpGet]
        public async Task<ApiResult> GetGroups([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetGroups(token);
        }

        [HttpPost]
        public async Task<ApiResult> SaveGroup([FromHeader] string token, GroupSaveWrapper groupSave)
        {
            return await new EntityLogic(ConfHelper).SaveGroup(token, groupSave);
        }

        [HttpDelete("{groupId}")]
        public async Task<ApiResult> DeleteGroups([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).DeleteGroups(token, groupId);
        }
    }
}

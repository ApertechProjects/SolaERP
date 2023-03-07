using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        public ConfHelper ConfHelper { get; }
        public UsersController(ConfHelper confHelper)
        {
            ConfHelper = confHelper;
        }

        [HttpGet]
        public async Task<ApiResult> GetUsers([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetUserList(token);
        }

        [HttpGet("{groupId}")]
        public async Task<ApiResult> GetUsersForGroup([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).GetUserListForGroup(token, groupId);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GroupController : CustomBaseController
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(int Id)
            => CreateActionResult(await _groupService.RemoveAsync(Id));
    }
}

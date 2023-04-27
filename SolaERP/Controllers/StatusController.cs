using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StatusController : CustomBaseController
    {
        private readonly IStatusService _statusService;
        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        /// <summary>
        ///Retrieve the available statuses for entities in the system.
        /// </summary>
        /// <remarks>The GetAll endpoint in the Status controller retrieves a list of the available statuses for entities in the system. This can be useful for determining which statuses are valid for a given entity type, and for providing users with a list of options to choose from when updating the status of an entity.<remarks>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => CreateActionResult(await _statusService.GetAllAsync());
    }
}

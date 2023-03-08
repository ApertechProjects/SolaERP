using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BusinessUnitController : CustomBaseController
    {
        private readonly IBusinessUnitService _businessUnitService;
        public BusinessUnitController(IBusinessUnitService businessUnitService)
        {
            _businessUnitService = businessUnitService;
        }

        /// <summary>
        ///Gets all Business unit List
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitList()
            => CreateActionResult(await _businessUnitService.GetAllAsync());

        /// <summary>
        ///Gets all Business units for User
        /// </summary>
        /// <remarks>Returns all business unit list which available for given User. (authToken = userIdentifier)</remarks>
        /// <param name="authToken">user identifier token</param>
        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitListByUser([FromHeader] string authToken)
            => CreateActionResult(await _businessUnitService.GetBusinessUnitListByUserToken(authToken));

        /// <summary>
        /// Gets all Business units for Group
        /// </summary>
        /// <remarks>Returns All Business Unit List which available for given groupId</remarks>
        /// <param name="groupId">groupd id for check</param>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetBusinessUnitListForGroups(int groupId)
            => CreateActionResult(await _businessUnitService.GetBusinessUnitForGroupAsync(groupId));
    }
}

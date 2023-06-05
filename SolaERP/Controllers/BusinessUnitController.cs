using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;

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


        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitList()
            => CreateActionResult(await _businessUnitService.GetAllAsync());


        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitListByUser()
            => CreateActionResult(await _businessUnitService.GetBusinessUnitListByUserToken(User.Identity.Name));
    }
}

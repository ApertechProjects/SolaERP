using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class BusinessUnitController : ControllerBase
    {
        private readonly IBusinessUnitService _businessUnitService;
        public BusinessUnitController(IBusinessUnitService businessUnitService)
        {
            _businessUnitService = businessUnitService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<BusinessUnitsAllDto>>> GetBusinessUnitList()
        {
            return await _businessUnitService.GetAllAsync();
        }

        [HttpGet("{token}")]
        public async Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId(string token)
        {
            return await _businessUnitService.GetBusinessUnitListByUserId(token);
        }
    }
}

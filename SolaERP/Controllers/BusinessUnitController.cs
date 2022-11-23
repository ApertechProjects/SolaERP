using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.BusinessUnit;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BusinessUnitController : ControllerBase
    {
        private readonly BusinessUnitService _businessUnitService;
        public BusinessUnitController(BusinessUnitService businessUnitService)
        {
            _businessUnitService = businessUnitService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<BusinessUnitsAllDto>>> GetBusinessUnitList()
        {
            return await _businessUnitService.GetAllAsync();
        }

        [HttpGet]
        public async Task<ApiResponse<List<BusinessUnitsDto>>> GetBusinessUnitListByUserId()
        {
            return await _businessUnitService.GetBusinessUnitListByUserId(Kernel.CurrentUserId);
        }
    }
}

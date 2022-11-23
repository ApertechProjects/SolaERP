using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
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
        public ApiResponse<List<BusinessUnitsAllDto>> GetBusinessUnitList()
        {
            return _businessUnitService.GetAll();
        }

        [HttpGet]
        public ApiResponse<List<BusinessUnitsDto>> GetBusinessUnitListByUserId()
        {
            return _businessUnitService.GetBusinessUnitListByUserId();
        }
    }
}

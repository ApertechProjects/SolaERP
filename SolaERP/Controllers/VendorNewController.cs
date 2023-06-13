using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorNewController : CustomBaseController
    {
        private readonly IVendorService _vendorService;
        public VendorNewController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> WaitingForApprovals(int businessUnitId)
            => CreateActionResult(await _vendorService.WaitingForApprovals(businessUnitId));
    }
}

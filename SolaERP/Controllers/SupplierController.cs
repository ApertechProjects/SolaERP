using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SupplierController : CustomBaseController
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        ///Gets a list of all supplier codes.
        /// </summary>
        /// <remarks>This endpoint returns a list of all supplier codes in the system. The response includes an array of SupplierCodeDto objects, each of which contains the supplier code and supplier name.</remarks>
        [HttpGet]
        public async Task<IActionResult> GetSupplierCodes()
            => CreateActionResult(await _supplierService.GetSupplierCodesAsync());
    }
}

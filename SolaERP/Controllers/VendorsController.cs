using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Dtos.EntityDtos.Vendor;
using SolaERP.Business.Models;
using SolaERP.Persistence.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorsController : CustomBaseController
    {
        private ConfHelper ConfHelper { get; }
        private readonly IUserRepository _userRepository;
        private readonly IVendorService _vendorService;
        public VendorsController(ConfHelper confHelper, IUserRepository userRepository, IVendorService vendorService)
        {
            ConfHelper = confHelper;
            _userRepository = userRepository;
            _vendorService = vendorService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> WaitingForApprovals()
            => CreateActionResult(await _vendorService.GetWFAAsync(User.Identity.Name, new()));

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> All()
        //  => CreateActionResult(await _vendorService.GetAll(User.Identity.Name));

        //[Authorize]
        //[HttpGet("{businessUnitId}")]
        //public async Task<IActionResult> Vendors(int businessUnitId)
        //    => CreateActionResult(await _vendorService.Vendors(businessUnitId, User.Identity.Name));


        [HttpGet("{vendorId}")]
        [Authorize]
        public async Task<ApiResult> GetVendorDetails(int vendorId)
        {
            return await new EntityLogic(ConfHelper).GetVendorDetails(User.Identity.Name, vendorId);
        }



        [HttpPost]
        [Authorize]
        public async Task<ApiResult> VendorApprove(List<VendorWFAModel> vendorWFAModel)
        {
            return await new EntityLogic(ConfHelper).VendorApprove(User.Identity.Name, vendorWFAModel);
        }




        [HttpPost]
        [Authorize]
        public async Task<ApiResult> VendorSendToApprove(VendorSendToApprove vendorSendToApprove)
        {
            return await new EntityLogic(ConfHelper).VendorSendToApprove(User.Identity.Name, vendorSendToApprove);
        }



        [Authorize]
        [HttpGet("{businessUnitId}")]
        public async Task<ApiResult> GetActiveVendor(int businessUnitId)
        {
            return await new EntityLogic(ConfHelper, _userRepository).GetActiveVendors(User.Identity.Name, businessUnitId);
        }

        [HttpGet("{taxId}")]
        public async Task<IActionResult> GetByTax(string taxId)
        {
            return Ok(await _vendorService.GetByTaxAsync(taxId));
        }

    }
}

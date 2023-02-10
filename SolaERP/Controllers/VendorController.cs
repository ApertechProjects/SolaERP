using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Dtos.EntityDtos.Vendor;
using SolaERP.Business.Models;
using SolaERP.Infrastructure.Contracts.Repositories;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private ConfHelper ConfHelper { get; }
        private readonly IUserRepository _userRepository;
        public VendorController(ConfHelper confHelper,IUserRepository userRepository)
        {
            ConfHelper = confHelper;
            _userRepository= userRepository;
        }


        [HttpGet("{BU}")]
        public async Task<ApiResult> GetWFAandAllVendorList([FromHeader] string token, int BU)
        {
            return await new EntityLogic(ConfHelper).GetVendorList(token, BU);
        }

        [HttpGet("{vendorId}")]
        public async Task<ApiResult> GetVendorDetails([FromHeader] string token, int vendorId)
        {
            return await new EntityLogic(ConfHelper).GetVendorDetails(token, vendorId);
        }



        [HttpPost]
        public async Task<ApiResult> VendorApprove([FromHeader] string token, List<VendorWFAModel> vendorWFAModel)
        {
            return await new EntityLogic(ConfHelper).VendorApprove(token, vendorWFAModel);
        }

        [HttpPost]
        public async Task<ApiResult> VendorSendToApprove([FromHeader] string token, VendorSendToApprove vendorSendToApprove)
        {
            return await new EntityLogic(ConfHelper).VendorSendToApprove(token, vendorSendToApprove);
        }


        [HttpGet("{businessUnitId}")]
        public async Task<ApiResult> GetActiveVendor([FromHeader] string authToken, int businessUnitId)
        {
            return await new EntityLogic(ConfHelper,_userRepository).GetActiveVendors(authToken, businessUnitId);
        }
    }
}

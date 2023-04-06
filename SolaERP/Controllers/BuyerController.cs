using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BuyerController : CustomBaseController
    {
        private readonly IBuyerService _buyerService;
        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        /// <summary>
        ///Get a list of buyers for the current user and business Unit Code
        /// </summary>
        /// <remarks>This endpoint returns a list of buyers that are associated with the currently authenticated user.</remarks>
        ///<param name="authToken">The token used to authenticate the user who performs the operation</param>
        ///<param name="businessUnitCode">The unique identifier of the business unit for which to retrieve account codes.</param>
        [HttpGet("{businessUnitCode}")]
        public async Task<IActionResult> GetBuyersByTokenAsync([FromHeader] string authToken, string businessUnitCode)
        => CreateActionResult(await _buyerService.GetBuyerByUserTokenAsync(authToken, businessUnitCode));

    }
}

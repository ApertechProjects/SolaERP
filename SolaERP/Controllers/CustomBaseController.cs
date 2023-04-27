using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ApiResponse<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null) { StatusCode = response.StatusCode };
            else
                return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Request;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<List<RequestMainDto>> GetAllMainRequest()
        {
            return await _requestService.GetAllAsync();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ApiResponse<List<UserDto>> GetAllUsers()
        {
            return _userService.GetAll();
        }
    }
}

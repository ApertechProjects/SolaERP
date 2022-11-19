using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.DataAccess.DataAcces.SqlServer;
using SolaERP.Infrastructure.Dtos;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SqlUserRepository _sqlUserRepository;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ApiResponse<List<UserDto>> GetAllUsers()
        {
            return _userService.GetAll();
        }

        [HttpPost]
        public ApiResponse<bool> AddUser(UserDto dto)
        {
            return _userService.Register(dto);
        }

        [HttpPut]
        public ApiResponse<bool> UpdateUser(UserDto dto)
        {
            return _userService.UpdateUser(dto);
        }

        [HttpDelete]
        public ApiResponse<bool> RemoveUser(UserDto dto)
        {
            return _userService.RemoveUser(dto);
        }
    }
}

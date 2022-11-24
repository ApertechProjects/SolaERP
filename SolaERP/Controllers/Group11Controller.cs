using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Group11Controller : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        public Group11Controller(IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<GroupsDto>>> GetGroups()
        {
            return await _groupService.GetAllAsync();
        }
    }
}

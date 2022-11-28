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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<GroupsDto>>> GetGroups()
        {
            return await _groupService.GetAllAsync();
        }

        [HttpDelete]
        public async Task<ApiResponse<bool>> DeleteGroups(GroupsDto groupsDto)
        {
            return await _groupService.RemoveAsync(groupsDto);
        }
    }
}

using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<GroupsDto> AddAsync(GroupsDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetAllAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            var dto = _mapper.Map<List<GroupsDto>>(groups);

            return ApiResponse<List<GroupsDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveAsync(GroupsDto model)
        {
            var dto = _mapper.Map<Groups>(model);
            _groupRepository.Remove(dto);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public Task<ApiResponse<bool>> UpdateAsync(GroupsDto model)
        {
            throw new NotImplementedException();
        }
    }
}

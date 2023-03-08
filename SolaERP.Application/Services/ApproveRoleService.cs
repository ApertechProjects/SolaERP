using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Application.Services
{
    public class ApproveRoleService : IApproveRoleService
    {
        private readonly IApproveRoleRepository _approveRoleRepository;
        private IMapper _mapper;
        public ApproveRoleService(IApproveRoleRepository approveRoleRepository, IMapper mapper)
        {
            _approveRoleRepository = approveRoleRepository;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveRoleDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ApproveRoleDto>>> GetAllAsync()
        {
            var approveRoles = await _approveRoleRepository.GetAllAsync();
            var dto = _mapper.Map<List<ApproveRoleDto>>(approveRoles);
            return ApiResponse<List<ApproveRoleDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveRoleDto model)
        {
            throw new NotImplementedException();
        }
    }
}

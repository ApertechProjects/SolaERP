using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class ApproveStageRoleService : IApproveStageRoleService
    {
        private readonly IApproveStageRoleRepository _approveStageRoleRepository;
        private IMapper _mapper;
        public ApproveStageRoleService(IApproveStageRoleRepository approveStageRoleRepository, IMapper mapper)
        {
            _approveStageRoleRepository = approveStageRoleRepository;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<ApproveStageRoleDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId)
        {
            var approveRoles = await _approveStageRoleRepository.GetApproveStageRolesByApproveStageDetailId(approveStageDetailsId);
            var dto = _mapper.Map<List<ApproveStageRoleDto>>(approveStageDetailsId);
            return ApiResponse<List<ApproveStageRoleDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }
    }
}

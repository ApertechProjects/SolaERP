using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class ApproveStageRoleService : IApproveStageRoleService
    {
        private readonly IApproveStageRoleRepository _approveStageRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ApproveStageRoleService(IApproveStageRoleRepository approveStageRoleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _approveStageRoleRepository = approveStageRoleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(ApproveStageRoleDto entity, int userId)
        {
            var model = _mapper.Map<ApproveStageRole>(entity);
            var role = await _approveStageRoleRepository.AddAsync(model, userId);
            return role;
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

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            _approveStageRoleRepository.Remove(Id);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public void Update(ApproveStageRoleDto entity, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }
    }
}

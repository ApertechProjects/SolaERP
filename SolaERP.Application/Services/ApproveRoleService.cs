using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class ApproveRoleService : IApproveRoleService
    {
        private readonly IApproveRoleRepository _approveRoleRepository;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        public ApproveRoleService(IApproveRoleRepository approveRoleRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _approveRoleRepository = approveRoleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task AddAsync(ApproveRoleDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> ApproveRoleSaveAsync(ApproveRoleDto model)
        {
            var dto = _mapper.Map<ApproveRole>(model);
            var roles = await _approveRoleRepository.ApproveRoleSaveAsync(dto);
            await _unitOfWork.SaveChangesAsync();
            if (roles)
                return ApiResponse<bool>.Success(roles, 200);
            return ApiResponse<bool>.Fail("approveRole", "Data can not be saved", 500);
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

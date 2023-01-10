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
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ApproveStageRoleService(IApproveStageRoleRepository approveStageRoleRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _approveStageRoleRepository = approveStageRoleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public Task<int> AddAsync(List<ApproveStageRoleDto> entities)
        {
            throw new NotImplementedException();
        }


        public async Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId)
        {
            var approveRoles = await _approveStageRoleRepository.GetApproveStageRolesByApproveStageDetailId(approveStageDetailsId);
            var dto = _mapper.Map<List<ApproveStageRoleDto>>(approveStageDetailsId);
            return ApiResponse<List<ApproveStageRoleDto>>.Success(dto, 200);
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            _approveStageRoleRepository.RemoveAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(string authToken, ApproveStageRoleDto entity)
        {
            var model = _mapper.Map<ApproveStageRole>(entity);
            var role = await _approveStageRoleRepository.AddAsync(model);
            return role;
        }

        public async Task<int> AddAsync(ApproveStageRoleDto entity)
        {
            var model = _mapper.Map<ApproveStageRole>(entity);
            var role = await _approveStageRoleRepository.AddAsync(model);
            return role;
        }

        public Task<ApiResponse<List<ApproveStageRoleDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task ICrudService<ApproveStageRoleDto>.AddAsync(ApproveStageRoleDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(string authToken, ApproveStageRoleDto entity)
        {
            throw new NotImplementedException();
        }
    }
}

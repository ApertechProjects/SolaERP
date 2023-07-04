using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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


        public async Task<ApiResponse<List<ApproveStageRoleDto>>> GetRoleAsync(int approveStageDetailsId)
        {
            var approveRoles = await _approveStageRoleRepository.GetByDetailIdAsync(approveStageDetailsId);
            var dto = _mapper.Map<List<ApproveStageRoleDto>>(approveRoles);
            return ApiResponse<List<ApproveStageRoleDto>>.Success(dto, 200);
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            await _approveStageRoleRepository.RemoveAsync(Id);
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

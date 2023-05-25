using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class ApproveRoleService : IApproveRoleService
    {
        private readonly IApproveRoleRepository _approveRoleRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        public ApproveRoleService(IApproveRoleRepository approveRoleRepository, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _approveRoleRepository = approveRoleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task AddAsync(ApproveRoleDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ApproveRoleDto>>> ApproveRoleAsync(int businessUnitId)
        {
            var data = await _approveRoleRepository.ApproveRoleAsync(businessUnitId);
            var map = _mapper.Map<List<ApproveRoleDto>>(data);
            return ApiResponse<List<ApproveRoleDto>>.Success(map, 200);
        }

        public async Task<ApiResponse<bool>> ApproveRoleDeleteAsync(ApproveRoleDeleteModel model, string userName)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(userName);
            var roles = false;
            int counter = 0;
            for (int i = 0; i < model.RoleIds.Count; i++)
            {
                roles = await _approveRoleRepository.ApproveRoleDeleteAsync(model.RoleIds[i], userId);
            }
            await _unitOfWork.SaveChangesAsync();
            if (model.RoleIds.Count == counter)
                return ApiResponse<bool>.Success(roles, 200);

            return ApiResponse<bool>.Fail("approveRole", "Data can not be deleted", 500);
        }

        public async Task<ApiResponse<bool>> ApproveRoleSaveAsync(List<ApproveRoleSaveModel> model, string userName)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(userName);
            var roles = false;
            int counter = 0;
            for (int i = 0; i < model.Count; i++)
            {
                roles = await _approveRoleRepository.ApproveRoleSaveAsync(model[i], userId);
                if (roles)
                    counter++;
            }

            await _unitOfWork.SaveChangesAsync();
            if (model.Count == counter)
                return ApiResponse<bool>.Success(roles, 200);

            return ApiResponse<bool>.Fail("approveRole", "Data can not be saved", 500);
        }

        public async Task<ApiResponse<List<ApproveRoleDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
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

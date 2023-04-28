using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using System.Data;


namespace SolaERP.Persistence.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddBusinessUnitsAsync(List<int> users, int groupId)
        {
            var data = users.ConvertListToDataTable();
            await _groupRepository.AddBusinessUnitsAsync(data, groupId);
        }
        public async Task DeleteBusinessUnitsAsync(List<int> users, int groupId)
        {
            var data = users.ConvertListToDataTable();
            await _groupRepository.DeleteBusinessUnitsAsync(data, groupId);
        }

        public async Task<ApiResponse<List<BusinessUnitForGroupDto>>> GetGroupBusinessUnitsAsync(int groupId)
        {
            var buisnessUnitForGroup = await _groupRepository.GetGroupBusinessUnitsAsync(groupId);
            var dto = _mapper.Map<List<BusinessUnitForGroupDto>>(buisnessUnitForGroup);

            return ApiResponse<List<BusinessUnitForGroupDto>>.Success(dto, 200);
        }

        public async Task AddUsersAsync(List<int> model, int groupId)
        {
            var data = model.ConvertListToDataTable();
            await _groupRepository.AddUsersAsync(data, groupId);
        }

        public async Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(int groupAnalysisCodeId)
        {
            var result = await _groupRepository.DeleteAnalysisCodeByGroupIdAsync(groupAnalysisCodeId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("Data can not be deleted", 400);
        }


        public async Task<ApiResponse<bool>> DeleteGroupRoleAsync(int groupApproveRoleId)
        {
            var result = await _groupRepository.DeleteGroupRoleAsync(groupApproveRoleId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("role", "Data can not be deleted", 400);
        }

        public async Task DeleteUsersAsync(List<int> users, int groupId)
        {
            var data = users.ConvertListToDataTable();
            await _groupRepository.DeleteUsersAsync(data, groupId);
        }

        public async Task<ApiResponse<List<GroupAdditionalPrivilegeDto>>> GetAdditionalPrivilegesAsync(int groupId)
        {
            var data = await _groupRepository.GetAdditionalPrivilegesAsync(groupId);
            var dto = _mapper.Map<List<GroupAdditionalPrivilegeDto>>(data);
            return ApiResponse<List<GroupAdditionalPrivilegeDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetAllAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            var dto = _mapper.Map<List<GroupsDto>>(groups);

            return ApiResponse<List<GroupsDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesAsync(int groupId)
        {
            var analysisCodes = await _groupRepository.GetAnalysisCodesAsync(groupId);
            var dto = _mapper.Map<List<GroupAnalysisCodeDto>>(analysisCodes);
            if (dto != null)
                return ApiResponse<List<GroupAnalysisCodeDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupAnalysisCodeDto>>.Fail("Analysis list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupBuyerDto>>> GetGroupBuyersAsync(int groupId)
        {
            var buyers = await _groupRepository.GetBuyersAsync(groupId);
            var dto = _mapper.Map<List<GroupBuyerDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupBuyerDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupBuyerDto>>.Fail("Buyer list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupEmailNotification>>> GetGroupEmailNotificationsAsync(int groupId)
        {
            var result = await _groupRepository.GetGroupEmailNotificationsAsync(groupId);
            return ApiResponse<List<GroupEmailNotification>>.Success(result, 200);
        }

        public async Task<ApiResponse<GroupDto>> GetGroupInfoAsync(int groupId)
        {
            var data = await _groupRepository.GetGroupInfoAsync(groupId);
            var dto = _mapper.Map<GroupDto>(data);
            if (dto != null)
                return ApiResponse<GroupDto>.Success(dto, 200);
            else
                return ApiResponse<GroupDto>.Fail("group not found", 404);
        }

        public async Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesAsync(int groupId)
        {
            var buyers = await _groupRepository.GetGroupRolesAsync(groupId);
            var dto = _mapper.Map<List<GroupRoleDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupRoleDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupRoleDto>>.Fail("Group Role list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupUserDto>>> GetUserGroupsAsync(int userId)
        {
            var groups = await _groupRepository.GetUserGroupsAsync(userId);
            var dto = _mapper.Map<List<GroupUserDto>>(groups);
            if (dto != null)
                return ApiResponse<List<GroupUserDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupUserDto>>.Fail("Group User list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetUserGroupsWithoutCurrents(int id)
        {
            var groups = await _groupRepository.GetUserGroupsAsync(id);
            var allGroups = await _groupRepository.GetAllAsync();
            var allGroupCanUserA = allGroups.Where(x => !allGroups.Any(y => y.GroupId == x.GroupId)).ToList();

            var dto = _mapper.Map<List<GroupsDto>>(allGroupCanUserA);

            return ApiResponse<List<GroupsDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> AddAnalysisCodeAsync(int groupId, List<int> analysisIds)
        {
            var res = await _groupRepository.AddAnalysisCodeAsync(groupId, analysisIds.ConvertListToDataTable());
            await _unitOfWork.SaveChangesAsync();

            if (res)
                return ApiResponse<bool>.Success(res, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> SaveGroupAsync(string identity, GroupSaveModel model)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(identity);
            model.GroupId = await _groupRepository.AddUpdateOrDeleteGroupAsync(userId, new() { GroupId = model.GroupId, GroupName = model.GroupName, Description = model.Description });

            if (model.AddUsers != null)
                await AddUsersAsync(model.AddUsers, model.GroupId);
            if (model.RemoveUsers != null)
                await DeleteUsersAsync(model.RemoveUsers, model.GroupId);

            if (model.AddBusinessUnits != null)
                await AddBusinessUnitsAsync(model.AddBusinessUnits, model.GroupId);
            if (model.RemoveBusinessUnits != null)
                await DeleteBusinessUnitsAsync(model.RemoveBusinessUnits, model.GroupId);

            if (model.AddApproveRoles != null)
                await AddApproveRolesAsync(model.AddApproveRoles, model.GroupId);
            if (model.RemoveApproveRoles != null)
                await DeleteApproveRolesAsync(model.RemoveApproveRoles, model.GroupId);

            if (model.AddAdditionalPrivileges != null)
                await AddAdditionalPrivilegesAsync(model.AddAdditionalPrivileges, model.GroupId);
            if (model.RemoveAdditionalPrivileges != null)
                await DeleteAdditionalPrivilegesAsync(model.RemoveAdditionalPrivileges, model.GroupId);

            if (model.AddBuyers != null)
                await AddBuyersAsync(model.AddBuyers, model.GroupId);
            if (model.RemoveBuyers != null)
                await DeleteBuyersAsync(model.RemoveBuyers, model.GroupId);

            if (model.AddMenus != null)
                await _groupRepository.AddMenuAsync(model.GroupId, model.AddMenus.ConvertToDataTable());
            if (model.RemoveMenus != null)
                await _groupRepository.DeleteMenuAsync(model.GroupId, model.RemoveMenus.ConvertListToDataTable());

            if (model.AddEmailNotification != null)
                await AddEmailNotificationsAsync(model.GroupId, model.AddEmailNotification);
            if (model.RemoveEmailNotification != null)
                await DeleteEmailNotificationAsync(model.GroupId, model.RemoveEmailNotification);

            if (model.AddAnalysisCodeIds != null)
                await _groupRepository.AddAnalysisCodeAsync(model.GroupId, model.AddAnalysisCodeIds.ConvertListToDataTable());
            if (model.RemoveAnalysisCodeIds != null)
                await _groupRepository.DeleteAnalysisCodeAsync(model.GroupId, model.RemoveAnalysisCodeIds.ConvertListToDataTable());

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        private async Task DeleteEmailNotificationAsync(int groupId, List<int> removeEmailNotification)
        {
            var data = removeEmailNotification.ConvertListToDataTable();
            await _groupRepository.DeleteEmailNotificationAsync(data, groupId);
        }

        private async Task AddEmailNotificationsAsync(int groupId, List<int> addEmailNotification)
        {
            var data = addEmailNotification.ConvertListToDataTable();
            await _groupRepository.AddEmailNotificationsAsync(data, groupId);
        }

        private async Task DeleteBuyersAsync(List<GroupBuyerSaveModel> removeBuyers, int groupId)
        {
            var data = removeBuyers.ConvertListToDataTable();
            await _groupRepository.DeleteBuyersAsync(data, groupId);
        }

        private async Task AddBuyersAsync(List<GroupBuyerSaveModel> addBuyers, int groupId)
        {
            var data = addBuyers.ConvertListToDataTable();
            await _groupRepository.AddBuyersAsync(data, groupId);
        }

        private async Task DeleteAdditionalPrivilegesAsync(List<int> removeAdditionalPrivileges, int groupId)
        {
            var data = removeAdditionalPrivileges.ConvertListToDataTable();
            await _groupRepository.DeleteAdditionalPrivilegesAsync(data, groupId);
        }

        private async Task AddAdditionalPrivilegesAsync(List<int> addAdditionalPrivileges, int groupId)
        {
            var data = addAdditionalPrivileges.ConvertListToDataTable();
            await _groupRepository.AddAdditionalPrivilegesAsync(data, groupId);
        }

        private async Task DeleteApproveRolesAsync(List<int> removeApproveRoles, int groupId)
        {
            var data = removeApproveRoles.ConvertListToDataTable();
            await _groupRepository.DeleteApproveRolesFromGroupAsync(data, groupId);
        }

        private async Task AddApproveRolesAsync(List<int> addApproveRoles, int groupId)
        {
            var data = addApproveRoles.ConvertListToDataTable();
            await _groupRepository.AddApproveRolesToGroupAsync(data, groupId);
        }

        public async Task<ApiResponse<bool>> SaveGroupRoleAsync(GroupRoleSaveModel model)
        {
            var res = await _groupRepository.SaveGroupRoleAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (res)
                return ApiResponse<bool>.Success(res, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> UpdateEmailNotificationAsync(GroupEmailNotification model)
        {
            var result = await _groupRepository.UpdateEmailNotificationAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return result ?
                            ApiResponse<bool>.Success(204)
                          : ApiResponse<bool>.Fail($"Something went wrong. The email notification was not updated.", 400);
        }

        public async Task<ApiResponse<bool>> DeleteGroupAsync(string identity, int groupId)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(identity);
            var data = await _groupRepository.AddUpdateOrDeleteGroupAsync(userId, new() { GroupId = groupId });

            if (data == 0)
                return ApiResponse<bool>.Success(200);
            else
                return ApiResponse<bool>.Fail($"Something went wrong. The email notification was not updated.", 400);
        }
    }
}

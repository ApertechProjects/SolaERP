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

        public async Task AddUsersAsync(List<int> model, int groupId)
        {
            var data = model.ConvertListToDataTable();
            await _groupRepository.AddUsersAsync(data, groupId);
        }

        public async Task<ApiResponse<bool>> CreateEmailNotificationAsync(CreateGroupEmailNotificationModel model)
        {
            var result = await _groupRepository.CreateEmailNotificationAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result ? ApiResponse<bool>.Success(204)
                          : ApiResponse<bool>.Fail($"Something went wrong. The email notification was not created for the group ID: {model.GroupId}", 500);
        }

        public async Task<ApiResponse<bool>> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId)
        {
            var result = await _groupRepository.DeleteAnalysisCodeByGroupIdAsync(groupAnalysisCodeId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("Data can not be deleted", 400);
        }


        public async Task<ApiResponse<bool>> DeleteBuyerByGroupIdAsync(int groupBuyerId)
        {
            var result = await _groupRepository.DeleteBuyerByGroupIdAsync(groupBuyerId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("Data can not be deleted", 400);
        }

        public async Task<ApiResponse<bool>> DeleteEmailNotificationAsync(int groupEmailNotificationId)
        {
            var result = await _groupRepository.DeleteEmailNotificationAsync(groupEmailNotificationId);
            await _unitOfWork.SaveChangesAsync();

            return result ?
                            ApiResponse<bool>.Success(204)
                          : ApiResponse<bool>.Fail($"The email notification with Id: {groupEmailNotificationId}  was not deleted.", 400);
        }

        public async Task<ApiResponse<bool>> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId)
        {
            var result = await _groupRepository.DeleteGroupRoleByGroupIdAsync(groupApproveRoleId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("role", "Data can not be deleted", 400);
        }

        public async Task DeleteUsersAsync(List<int> users, int groupId)
        {
            var data = users.ConvertListToDataTable();
            await _groupRepository.DeleteUsersAsync(data, groupId);
        }

        public async Task<ApiResponse<List<GroupAdditionalPrivilage>>> GetAdditionalPrivilegesForGroupAsync(int groupId)
        {
            return ApiResponse<List<GroupAdditionalPrivilage>>.Success(await _groupRepository.GetAdditionalPrivilegesForGroupAsync(groupId), 200);
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetAllAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            var dto = _mapper.Map<List<GroupsDto>>(groups);

            return ApiResponse<List<GroupsDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesByGroupIdAsync(int groupId)
        {
            var buyers = await _groupRepository.GetBuyersByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<GroupAnalysisCodeDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupAnalysisCodeDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupAnalysisCodeDto>>.Fail("Analysis list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupBuyerDto>>> GetBuyersByGroupIdAsync(int groupId)
        {
            var buyers = await _groupRepository.GetBuyersByGroupIdAsync(groupId);
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

        public async Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesByGroupIdAsync(int groupId)
        {
            var buyers = await _groupRepository.GetGroupRolesByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<GroupRoleDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupRoleDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupRoleDto>>.Fail("Group Role list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupUserDto>>> GetGroupsByUserIdAsync(int userId)
        {
            var groups = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var dto = _mapper.Map<List<GroupUserDto>>(groups);
            if (dto != null)
                return ApiResponse<List<GroupUserDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupUserDto>>.Fail("Group User list is empty", 404);
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetUserGroupsWithoutCurrents(int id)
        {
            var groups = await _groupRepository.GetGroupsByUserIdAsync(id);
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

        public async Task<ApiResponse<bool>> SaveGroupAsync(string name, GroupSaveModel model)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
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
            //if (model.ApproveRoles != null)
            //{
            //    if (model.GroupId == 0) await _groupRepository.AddApproveRoleAsync(model.GroupId, 0); // for delete operation approvalId is 0
            //    foreach (var approveRole in model.ApproveRoles)
            //    {
            //        await _groupRepository.AddApproveRoleAsync(model.GroupId, approveRole);
            //    }
            //}

            //if (model.AdditionalPrivilege != null)
            //{
            //    await _groupRepository.AdditionalPrivilegeAddOrUpdateAsync(new() { GroupAdditionalPrivilegeId = model.AdditionalPrivilege.GroupAdditionalPrivilegeId });
            //    await _groupRepository.AdditionalPrivilegeAddOrUpdateAsync(new()
            //    {
            //        GroupAdditionalPrivilegeId = model.AdditionalPrivilege.GroupAdditionalPrivilegeId,
            //        GroupId = model.GroupId,
            //        VendorDraft = model.AdditionalPrivilege.VendorDraft
            //    });
            //}

            //if (model.Menus != null)
            //{
            //    var menuIds = model.Menus.GetAllUnionMenuIds(); // gets all menu ids in a union list
            //    if (model.GroupId == 0) await _groupRepository.AddMenuAsync(new() { GroupId = model.GroupId });

            //    foreach (var menuId in menuIds)
            //    {
            //        await _groupRepository.AddMenuAsync(new()
            //        {
            //            GroupId = model.GroupId,
            //            MenuId = menuId,
            //            Create = GetMenuIdforAction(model.Menus, MenuAction.Create, menuId),
            //            Edit = GetMenuIdforAction(model.Menus, MenuAction.Edit, menuId),
            //            Delete = GetMenuIdforAction(model.Menus, MenuAction.Delete, menuId),
            //            Export = GetMenuIdforAction(model.Menus, MenuAction.Export, menuId)
            //        });
            //    }
            //}
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        private async Task DeleteBuyersAsync(List<GroupBuyerSaveModel> removeBuyers, int groupId)
        {
            var data = removeBuyers.ConvertListCollectionToDataTable();
            await _groupRepository.DeleteBuyersAsync(data, groupId);
        }

        private async Task AddBuyersAsync(List<GroupBuyerSaveModel> addBuyers, int groupId)
        {
            var data = addBuyers.ConvertListCollectionToDataTable();
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

        public async Task<ApiResponse<bool>> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model)
        {
            var res = await _groupRepository.SaveGroupRoleByGroupAsync(model);
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

        private int GetMenuIdforAction(GroupMenuPrivilegeListModel menus, MenuAction action, int menuId)
        {
            switch (action)
            {
                case MenuAction.Create:
                    return Convert.ToInt16(menus.Create.Contains(menuId));
                case MenuAction.Edit:
                    return Convert.ToInt16(menus.Edit.Contains(menuId));
                case MenuAction.Delete:
                    return Convert.ToInt16(menus.Delete.Contains(menuId));
                default:
                    return Convert.ToInt16(menus.Export.Contains(menuId));
            }
        }

    }
}

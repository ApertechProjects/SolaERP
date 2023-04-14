using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Enums;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
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

        public async Task<ApiResponse<bool>> AddOrUpdateAsync(GroupAdditionalPrivelegeDto additionalPrivilage)
        {
            bool isSucces = false;

            if (additionalPrivilage != null)
            {
                var entity = _mapper.Map<GroupAdditionalPrivilage>(additionalPrivilage);
                isSucces = await _groupRepository.AdditionalPrivilegeAddOrUpdateAsync(entity);
            }
            return ApiResponse<bool>.Success(isSucces, 200);
        }

        public async Task<ApiResponse<bool>> AddUserToGroupAsync(AddUserToGroupModel model)
        {
            var data = await _groupRepository.AddUserToGroupAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (data)
                return ApiResponse<bool>.Success(data, 200);
            return ApiResponse<bool>.Fail("user", "Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> CreateEmailNotficationAsync(CreateGroupEmailNotficationModel model)
        {
            var result = await _groupRepository.CreateEmailNotficationAsync(model);
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

        public async Task<ApiResponse<bool>> DeleteEmailNotficationAsync(int groupEmailNotficationId)
        {
            var result = await _groupRepository.DeleteEmailNotficationAsync(groupEmailNotficationId);
            await _unitOfWork.SaveChangesAsync();

            return result ?
                            ApiResponse<bool>.Success(204)
                          : ApiResponse<bool>.Fail($"The email notification with Id: {groupEmailNotficationId}  was not deleted.", 500);
        }

        public async Task<ApiResponse<bool>> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId)
        {
            var result = await _groupRepository.DeleteGroupRoleByGroupIdAsync(groupApproveRoleId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("role", "Data can not be deleted", 400);
        }

        public async Task<ApiResponse<bool>> DeleteUserFromGroupAsync(int groupUserId)
        {
            var result = await _groupRepository.DeleteUserFromGroupAsync(groupUserId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("user", "Data can not be deleted", 400);
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
                return ApiResponse<List<GroupAnalysisCodeDto>>.Fail("Analysis list is empty", 400);
        }

        public async Task<ApiResponse<List<GroupBuyerDto>>> GetBuyersByGroupIdAsync(int groupId)
        {
            var buyers = await _groupRepository.GetBuyersByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<GroupBuyerDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupBuyerDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupBuyerDto>>.Fail("Buyer list is empty", 400);
        }

        public async Task<ApiResponse<List<GroupEmailNotfication>>> GetGroupEmailNotficationsAsync(int groupId)
        {
            var result = await _groupRepository.GetGroupEmailNotficationsAsync(groupId);
            return ApiResponse<List<GroupEmailNotfication>>.Success(result, 200);
        }

        public async Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesByGroupIdAsync(int groupId)
        {
            var buyers = await _groupRepository.GetGroupRolesByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<GroupRoleDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupRoleDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupRoleDto>>.Fail("Group Role list is empty", 400);
        }

        public async Task<ApiResponse<List<GroupUserDto>>> GetGroupsByUserIdAsync(int userId)
        {
            var groups = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var dto = _mapper.Map<List<GroupUserDto>>(groups);
            if (dto != null)
                return ApiResponse<List<GroupUserDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupUserDto>>.Fail("Group User list is empty", 400);
        }

        public async Task<ApiResponse<List<GroupsDto>>> GetUserGroupsWithoutCurrents(int id)
        {
            var groups = await _groupRepository.GetGroupsByUserIdAsync(id);
            var allGroups = await _groupRepository.GetAllAsync();
            var allGroupCanUserA = allGroups.Where(x => !allGroups.Any(y => y.GroupId == x.GroupId)).ToList();

            var dto = _mapper.Map<List<GroupsDto>>(allGroupCanUserA);

            return ApiResponse<List<GroupsDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model)
        {
            var res = await _groupRepository.SaveAnalysisCodeByGroupAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (res)
                return ApiResponse<bool>.Success(res, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
        {
            var res = await _groupRepository.SaveBuyerByGroupAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (res)
                return ApiResponse<bool>.Success(res, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> SaveGroupAsync(string finderToken, GroupSaveModel model)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            model.GroupId = await _groupRepository.AddUpdateOrDeleteGroupAsync(userId, new() { GroupId = model.GroupId, GroupName = model.GroupName, Description = model.Description });

            if (model.Users != null)
            {
                model.Users.Add(userId);

                if (model.GroupId == 0) await _groupRepository.AddUserToGroupOrDeleteAsync(new() { GroupId = model.GroupId });
                foreach (var user in model.Users)
                {
                    await _groupRepository.AddUserToGroupOrDeleteAsync(new() { GroupId = model.GroupId, UserId = user });
                }
            }

            if (model.BusinessUnitIds != null)
            {
                if (model.GroupId == 0) await _groupRepository.AddBusiessUnitToGroupOrDeleteAsync(model.GroupId, 0); // for delete operation buid is 0
                foreach (var buId in model.BusinessUnitIds)
                {
                    await _groupRepository.AddBusiessUnitToGroupOrDeleteAsync(model.GroupId, buId);
                }
            }

            if (model.ApproveRoles != null)
            {
                if (model.GroupId == 0) await _groupRepository.AddApproveRoleToGroupOrDelete(model.GroupId, 0); // for delete operation approvalId is 0
                foreach (var approveRole in model.ApproveRoles)
                {
                    await _groupRepository.AddApproveRoleToGroupOrDelete(model.GroupId, approveRole);
                }
            }

            if (model.AdditionalPrivilege != null)
            {
                await _groupRepository.AdditionalPrivilegeAddOrUpdateAsync(new() { GroupAdditionalPrivilegeId = model.AdditionalPrivilege.GroupAdditionalPrivilegeId });
                await _groupRepository.AdditionalPrivilegeAddOrUpdateAsync(new()
                {
                    GroupAdditionalPrivilegeId = model.AdditionalPrivilege.GroupAdditionalPrivilegeId,
                    GroupId = model.GroupId,
                    VendorDraft = model.AdditionalPrivilege.VendorDraft
                });
            }

            if (model.Menus != null)
            {
                var menuIds = model.Menus.GetAllUnionMenuIds(); // gets all menu ids in a union list
                if (model.GroupId == 0) await _groupRepository.AddMenuToGroupOrDeleteAsync(new() { GroupId = model.GroupId });

                foreach (var menuId in menuIds)
                {
                    await _groupRepository.AddMenuToGroupOrDeleteAsync(new()
                    {
                        GroupId = model.GroupId,
                        MenuId = menuId,
                        Create = GetMenuIdforAction(model.Menus, MenuAction.Create, menuId),
                        Edit = GetMenuIdforAction(model.Menus, MenuAction.Edit, menuId),
                        Delete = GetMenuIdforAction(model.Menus, MenuAction.Delete, menuId),
                        Export = GetMenuIdforAction(model.Menus, MenuAction.Export, menuId)
                    });
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
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

        public async Task<ApiResponse<bool>> UpdateEmailNotficationAsync(GroupEmailNotfication model)
        {
            var result = await _groupRepository.UpdateEmailNotficationAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return result ?
                            ApiResponse<bool>.Success(204)
                          : ApiResponse<bool>.Fail($"Something went wrong. The email notification was not updated.", 500);
        }

        /// <summary>
        /// Checks if a menuid contains in a list. if is, returns menu id for this action
        /// </summary>
        /// <param name="action">Menu actions</param>
        /// <param name="menus">Menu model which contains List of menu id for Menuactions</param>
        /// <param name="menuId">menuid for checking </param>
        /// <returns></returns>
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

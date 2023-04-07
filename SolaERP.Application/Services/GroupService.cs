using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
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

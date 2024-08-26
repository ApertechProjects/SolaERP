using SolaERP.Application.Constants;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Group = SolaERP.Application.Entities.Groups.Group;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGroupRepository : SqlBaseGroupRepository, IGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlGroupRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddMenuAsync(int groupId, DataTable table)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupMenuBulk_I  @GroupId,@MenuType";

                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@MenuType", GroupRepoConstants.AddMenu, table);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }
        public async Task<List<BusinessUnitForGroup>> GetGroupBusinessUnitsAsync(int groupId)
        {
            List<BusinessUnitForGroup> businessUnitForGroups = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_GroupBusinessUnit_Load";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    businessUnitForGroups.Add(reader.GetByEntityStructure<BusinessUnitForGroup>());

                return businessUnitForGroups;
            }
        }


        public async Task<List<GroupAdditionalPrivilege>> GetAdditionalPrivilegesAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupAdditionalPrivileges_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();

                List<GroupAdditionalPrivilege> resultList = new();
                while (reader.Read()) resultList.Add(reader.GetByEntityStructure<GroupAdditionalPrivilege>());

                return resultList;
            }
        }

        public async Task<List<Groups>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_GroupMain_Load";
                using var reader = await command.ExecuteReaderAsync();

                List<Groups> groups = new List<Groups>();
                while (reader.Read())
                {
                    groups.Add(reader.GetByEntityStructure<Groups>());
                }
                return groups;
            }
        }

        public async Task<int> AddUpdateOrDeleteGroupAsync(int userID, Groups entity)
        {
            int res = 0;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_Groups_IUD @GroupId,@GroupName,@Description,@UserId,@NewId OUTPUT";//User id cnat be null or 0

                command.Parameters.AddWithValue(command, "@GroupId", entity.GroupId);
                command.Parameters.AddWithValue(command, "@GroupName", entity.GroupName);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@UserId", userID);

                var result = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(result);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    res = result.Value == DBNull.Value ? 0 : (int)result.Value;
                }


                return res;
            }
        }

        public async Task AddBusinessUnitAsync(int groupId, int busiessUnitId)
        {
            using (var commmand = _unitOfWork.CreateCommand() as DbCommand)
            {
                commmand.CommandText = "EXEC SP_GroupBusinessUnits_ID @GroupId,@BusinessUnitId";
                commmand.Parameters.AddWithValue(commmand, "@GroupId", groupId);
                commmand.Parameters.AddWithValue(commmand, "@BusinessUnitId", busiessUnitId);

                await commmand.ExecuteNonQueryAsync();
            }
        }

        public async Task AddApproveRoleAsync(int groupId, int approveRoleId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupApproveRoles_ID @GroupId,@ApproveRoleId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddWithValue(command, "@ApproveRoleId", approveRoleId);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupBuyers_IUD @GroupBuyerId";
                command.Parameters.AddWithValue(command, "@GroupBuyerId", groupBuyerId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupBuyer>> GetBuyersAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_GroupBuyers_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                using var reader = await command.ExecuteReaderAsync();

                List<GroupBuyer> buyers = new List<GroupBuyer>();

                while (reader.Read())
                {
                    buyers.Add(reader.GetByEntityStructure<GroupBuyer>());
                }
                return buyers;
            }
        }

        public async Task<List<GroupAnalysisCode>> GetAnalysisCodesAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupAnalysisCodes_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                List<GroupAnalysisCode> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<GroupAnalysisCode>());

                return resultList;
            }
        }

        public async Task<bool> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupAnalysisCodes_IUD  @GroupAnalysisCodeId";
                command.Parameters.AddWithValue(command, "@GroupAnalysisCodeId", groupAnalysisCodeId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddAnalysisCodeAsync(int groupId, DataTable table)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupAnalysisBulk_I @GroupId,@Items";

                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", GroupRepoConstants.AddAnalysisCode, table);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupRole>> GetGroupRolesAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupApproveRoles_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                List<GroupRole> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<GroupRole>());

                return resultList;
            }
        }

        public async Task<bool> SaveGroupRoleAsync(GroupRoleSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupApproveRoles_IUD  @GroupApproveRoleId,@GroupId,@ApproveRoleId";

                command.Parameters.AddWithValue(command, "@GroupApproveRoleId", model.GroupApproveRoleId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@ApproveRoleId", model.ApproveRoleId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteGroupRoleAsync(int groupApproveRoleId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupApproveRoles_IUD @GroupApproveRoleId";
                command.Parameters.AddWithValue(command, "@GroupApproveRoleId", groupApproveRoleId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupUser>> GetUserGroupsAsync(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_UserGroupList @UserId";
                command.Parameters.AddWithValue(command, "@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                List<GroupUser> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<GroupUser>());

                return resultList;
            }
        }

        public async Task<List<GroupEmailNotification>> GetGroupEmailNotificationsAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupEmailNotification_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                List<GroupEmailNotification> resultList = new();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<GroupEmailNotification>());

                return resultList;
            }
        }

        public async Task<bool> CreateEmailNotificationAsync(CreateGroupEmailNotificationModel entity)
        {
            return await SaveEmailNotificationAsync(new()
            {
                GroupEmailNotificationId = 0,
                GroupId = entity.GroupId,
                EmailNotificationId = entity.EmailNotificationId,
            });
        }

        public async Task<bool> UpdateEmailNotificationAsync(GroupEmailNotification entity)
        {
            return await SaveEmailNotificationAsync(entity);
        }

        public async Task<bool> DeleteEmailNotificationAsync(int groupEmailNotificationId)
        {
            return await SaveEmailNotificationAsync(new() { GroupEmailNotificationId = groupEmailNotificationId });
        }

        protected override async Task<bool> SaveEmailNotificationAsync(GroupEmailNotification entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupEmailNotification_IUD @GroupEmailNotificationId,@GroupId,@EmailNotificationId";
                command.Parameters.AddWithValue(command, "@GroupEmailNotificationId", entity.GroupEmailNotificationId);
                command.Parameters.AddWithValue(command, "@GroupId", entity.GroupId);
                command.Parameters.AddWithValue(command, "@EmailNotificationId", entity.EmailNotificationId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<Group> GetGroupInfoAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec [dbo].[SP_GroupHeader_Load] @groupId";
                command.Parameters.AddWithValue(command, "@groupId", groupId);

                using var reader = await command.ExecuteReaderAsync();
                Group group = new Group();

                while (reader.Read())
                    group = reader.GetByEntityStructure<Group>();

                return group;
            }
        }

        public async Task AddUsersAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupUsersBulk_I @GroupId,@UserId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@UserId", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteUsersAsync(DataTable model, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupUsersBulk_D @GroupId,@UserId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@UserId", "SingleIdItems", model);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task AddBusinessUnitsAsync(DataTable model, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupBusinessUnitBulk_I @GroupId,@UserId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@UserId", "SingleIdItems", model);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteBusinessUnitsAsync(DataTable model, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupBusinessUnitsBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", model);
                var value = await command.ExecuteNonQueryAsync();
            }
        }
        public async Task AddApproveRolesToGroupAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupApproveRolesBulk_I @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteApproveRolesFromGroupAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupApproveRolesBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAdditionalPrivilegesAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupAdditionalPrivilegesBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task AddAdditionalPrivilegesAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupAdditionalPrivilegesBulk_I @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task AddBuyersAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupBuyersBulk_I @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "GroupBuyersType", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteBuyersAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupBuyersBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "GroupBuyersType", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteEmailNotificationAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupEmailNotificationsBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task AddEmailNotificationsAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupEmailNotificationsBulk_I @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", data);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> DeleteMenuAsync(int groupId, DataTable table)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupMenuBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", table);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAnalysisCodeAsync(int groupId, DataTable table)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupAnalysisBulk_D @GroupId,@Items";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                command.Parameters.AddTableValue(command, "@Items", "SingleIdItems", table);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<int> GetGroupIdByVendorAdmin()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT GroupId from Config.Groups where GroupName = 'Vendor Admin'";
                using var reader = await command.ExecuteReaderAsync();

                int id = 0;
                if (await reader.ReadAsync())
                {
                    id = reader.GetInt32("GroupId");
                }
                return id;
            }

        }
    }
}

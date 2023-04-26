using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Entities.User;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using Group = SolaERP.Infrastructure.Entities.Groups.Group;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGroupRepository : SqlBaseGroupRepository, IGroupRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlGroupRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddMenuToGroupOrDeleteAsync(GroupMenuIDSaveModel saveOrDeleteModel)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupMenus_ID @GroupId,@MenuId,@Edit,@Delete,@Export";
                command.Parameters.AddWithValue(command, "@GroupId", saveOrDeleteModel.GroupId);
                command.Parameters.AddWithValue(command, "@MenuId", saveOrDeleteModel.MenuId);
                command.Parameters.AddWithValue(command, "@Edit", saveOrDeleteModel.Edit);
                command.Parameters.AddWithValue(command, "@Delete", saveOrDeleteModel.Delete);
                command.Parameters.AddWithValue(command, "@Export", saveOrDeleteModel.Export);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> AdditionalPrivilegeAddOrUpdateAsync(GroupAdditionalPrivilage additionalPrivilage)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupAdditionalPrivileges_IUD @GroupAddtionalPrivilegeId,@GroupId,@VendorDraft,@RequestAttachment,@RequstSendToApprove";
                command.Parameters.AddWithValue(command, "@GroupAddtionalPrivilegeId", additionalPrivilage.GroupAdditionalPrivilegeId);
                command.Parameters.AddWithValue(command, "@GroupId", additionalPrivilage.GroupId);
                command.Parameters.AddWithValue(command, "@VendorDraft", additionalPrivilage.VendorDraft);
                command.Parameters.AddWithValue(command, "@RequestAttachment", additionalPrivilage.RequestAttachment);
                command.Parameters.AddWithValue(command, "@RequstSendToApprove", additionalPrivilage.RequestSendToApprove);

                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<GroupAdditionalPrivilage>> GetAdditionalPrivilegesForGroupAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupAdditionalPrivileges_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                using var reader = await command.ExecuteReaderAsync();

                List<GroupAdditionalPrivilage> resultList = new();
                while (reader.Read()) resultList.Add(reader.GetByEntityStructure<GroupAdditionalPrivilage>("RequestAttachment", "RequestSendToApprove"));

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
                command.CommandText = "EXEC SP_Groups_IUD @GroupId,@GroupName,@Description,@UserId,@NewId OUTPUT";//User id cnat be null or 0

                command.Parameters.AddWithValue(command, "@GroupId", entity.GroupId);
                command.Parameters.AddWithValue(command, "@GroupName", entity.GroupName);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@UserId", userID);

                var result = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(result);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    res = (int)result.Value;
                }


                return res;
            }
        }

        public async Task AddBusiessUnitToGroupOrDeleteAsync(int groupId, int busiessUnitId)
        {
            using (var commmand = _unitOfWork.CreateCommand() as DbCommand)
            {
                commmand.CommandText = "EXEC SP_GroupBusinessUnits_ID @GroupId,@BusinessUnitId";
                commmand.Parameters.AddWithValue(commmand, "@GroupId", groupId);
                commmand.Parameters.AddWithValue(commmand, "@BusinessUnitId", busiessUnitId);

                await commmand.ExecuteNonQueryAsync();
            }
        }

        public async Task AddApproveRoleToGroupOrDelete(int groupId, int approveRoleId)
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

        public async Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId)
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

        public async Task<bool> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupBuyers_IUD  @GroupBuyerId,
                                                                       @GroupId,  
                                                                       @BusinessUnitId, 
                                                                       @BuyerCode";

                command.Parameters.AddWithValue(command, "@GroupBuyerId", model.GroupBuyerId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@BuyerCode", model.BuyerCode);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId)
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

        public async Task<bool> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupAnalysisCodes_IUD  @GroupAnalysisCodeId,
                                                                       @GroupId,  
                                                                       @BusinessUnitId, 
                                                                       @AnalysisDimensionId, 
                                                                       @AnalysisCodesId";

                command.Parameters.AddWithValue(command, "@GroupAnalysisCodeId", model.GroupAnalysisCodeId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@AnalysisDimensionId", model.AnalysisDimensionId);
                command.Parameters.AddWithValue(command, "@AnalysisCodesId", model.AnalysisCodesId);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupRole>> GetGroupRolesByGroupIdAsync(int groupId)
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

        public async Task<bool> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupApproveRoles_IUD  @GroupApproveRoleId,
                                                                       @GroupId,  
                                                                       @ApproveRoleId";

                command.Parameters.AddWithValue(command, "@GroupApproveRoleId", model.GroupApproveRoleId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@ApproveRoleId", model.ApproveRoleId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupApproveRoles_IUD @GroupApproveRoleId";
                command.Parameters.AddWithValue(command, "@GroupApproveRoleId", groupApproveRoleId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<GroupUser>> GetGroupsByUserIdAsync(int userId)
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

        private Groups GetGroupData(DbDataReader reader)
        {
            return new()
            {
                GroupId = reader.Get<int>("GroupId"),
                GroupName = reader.Get<string>("GroupName"),
                Description = reader.Get<string>("Description")
            };
        }

        public async Task AddUserToGroupAsync(DataTable data, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupUsersBulk_I @GroupId,@UserId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                command.Parameters.Add("@UserId", SqlDbType.Structured).Value = data;
                command.Parameters["@UserId"].TypeName = "SingleIdItems";
                var value = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteUserToGroupAsync(DataTable model, int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_GroupUsersBulk_D @GroupId,@UserId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);

                command.Parameters.Add("@UserId", SqlDbType.Structured).Value = model;
                command.Parameters["@UserId"].TypeName = "SingleIdItems";
                var value = await command.ExecuteNonQueryAsync();
            }
        }
    }
}

using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGroupRepository : IGroupRepository
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
                while (reader.Read()) resultList.Add(reader.GetByEntityStructure<GroupAdditionalPrivilage>());

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
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_Groups_IUD @GroupId,@GroupName,@Description,@UserId,@NewId OUTPUT";//User id cnat be null or 0

                command.Parameters.AddWithValue(command, "@GroupId", entity.GroupId);
                command.Parameters.AddWithValue(command, "@GroupName", entity.GroupName);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@UserId", userID);
                command.Parameters.AddOutPutParameter(command, "@NewId");

                int result = 0;
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync()) result = reader.Get<int>("");

                return result;
            }
        }

        public async Task AddUserToGroupOrDeleteAsync(UserToGroupModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GroupUsers_ID @GroupId,@UserId";

                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@UserId", model.UserId);

                await command.ExecuteNonQueryAsync();
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
    }
}

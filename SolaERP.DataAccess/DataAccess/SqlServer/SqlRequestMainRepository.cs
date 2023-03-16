using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlRequestMainRepository : SqlBaseRepository, IRequestMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlRequestMainRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DeleteAsync(int userId, int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "EXEC SP_RequestMain_IUD @RequestMainId,NULL,NULL,NULL,NULL,NULL,@UserId, @NewRequestmainId = @NewRequestmainId OUTPUT, @NewRequestNo = @NewRequestNo OUTPUT select @NewRequestmainId as NewRequestmainId, @NewRequestNo as NewRequestNo";
                command.Parameters.AddWithValue(command, "@RequestMainId", Id);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.Add("@NewRequestMainId", SqlDbType.Int);
                command.Parameters["@NewRequestMainId"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@NewRequestNo", SqlDbType.NVarChar, 20);
                command.Parameters["@NewRequestNo"].Direction = ParameterDirection.Output;

                await command.ExecuteNonQueryAsync();

                var returnValue = command.Parameters["@NewRequestMainId"].Value;
                return returnValue != DBNull.Value && returnValue != null ? Convert.ToInt32(returnValue) : 0;
            }
        }

        public async Task<int> AddOrUpdateAsync(RequestMainSaveModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RequestTypes>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_RequestTypesByBuId @BusinessUnitId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestTypes> requestTypes = new List<RequestTypes>();

                while (reader.Read())
                    requestTypes.Add(reader.GetByEntityStructure<RequestTypes>());

                return requestTypes;
            }

        }

        public async Task<bool> RequestMainChangeStatusAsync(int userId, int requestMainId, int approveStatus, string comment)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMainApprove @UserId,@RequestMainId,@ApproveStatus,@Comment";

                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApproveStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@Comment", comment);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RequestMainDraft>> GetMainRequestDraftsAsync(RequestMainDraftModel requestMain)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainDrafts]", new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", string.Join(',', requestMain.ItemCodes));
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "DateTo", requestMain.DateTo);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMainDraft> mainDrafts = new List<RequestMainDraft>();
                while (reader.Read())
                    mainDrafts.Add(GetMainDraftFromReader(reader));
                return mainDrafts;
            }
        }

        public async Task<bool> SendRequestToApproveAsync(int userId, int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestSendToApprove @UserId,@RequestMainId";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<RequestMain> GetRequestByRequestMainIdAsync(int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from Procurement.RequestMain where RequestMainId = @Id";

                command.Parameters.AddWithValue(command, "@Id", requestMainId);

                using var reader = await command.ExecuteReaderAsync();

                RequestMain mainRequests = new RequestMain();
                while (reader.Read())
                {
                    mainRequests = GetAllFromReader(reader);
                }
                return mainRequests;
            }
        }

        public async Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, RequestWFAGetModel requestWFA)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainWFA]", new ReplaceParams { ParamName = "APT", Value = requestWFA.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestWFA.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestWFA.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestWFA.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", string.Join(',', requestWFA.ItemCode));

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetWaitingForApprovalFromReader(reader));
                }

                return mainRequests;
            }
        }

        public async Task<List<RequestAmendment>> GetApproveAmendmentRequestsAsync(int userId, RequestApproveAmendmentModel requestParametersDto)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainApproveAmendment]", new ReplaceParams { ParamName = "APT", Value = requestParametersDto.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestParametersDto.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestParametersDto.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestParametersDto.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", string.Join(',', requestParametersDto.ItemCodes));

                List<RequestAmendment> mainRequestsForAmendment = new();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    mainRequestsForAmendment.Add(GetRequestAmendmentFromReader(reader));
                return mainRequestsForAmendment;
            }
        }


        public async Task<RequestCardMain> GetRequesMainHeaderAsync(int requestMainId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestHeader_Load @RequestMainId,@UserId";
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                RequestCardMain result = null;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read()) result = GetRequestCardFromReader(reader);
                else
                    result = new();
                return result;
            }
        }

        public async Task<List<RequestApprovalInfo>> GetRequestApprovalInfoAsync(int requestMainId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestApprovalInfo @RequestMainId,@UserId";
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestApprovalInfo> resultList = new();
                while (await reader.ReadAsync()) resultList.Add(reader.GetByEntityStructure<RequestApprovalInfo>());

                return resultList;
            }
        }

        private RequestMain GetAllFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                Status = reader.Get<int>("Status"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
            };
        }



        private RequestMain GetWaitingForApprovalFromReader(IDataReader reader)
        {
            return new()
            {
                RowNum = reader.Get<Int64>("RowNum"),
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                ApproveStatus = reader.Get<int>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                Buyer = reader.Get<string>("Buyer"),
                BuyerName = reader.Get<string>("BuyerName"),
                RequestType = reader.Get<string>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
            };
        }

        private RequestCardMain GetRequestCardFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
                Destination = reader.Get<int>("Destination"),
                ApproveStatus = reader.Get<string>("ApproveStatus")

            };
        }

        private RequestAmendment GetRequestAmendmentFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticTotal = reader.Get<int>("LogisticTotal"),

            };
        }

        private RequestMainDraft GetMainDraftFromReader(DbDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                RowNum = reader.Get<Int64>("RowNum"),
                RequestType = reader.Get<string>("RequestType"),
                RequetsNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                ApproveStatus = reader.Get<int>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("ApproveStatusName")
            };
        }

        public async Task<RequestSaveResultModel> AddOrUpdateRequestAsync(int userId, RequestMainSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_RequestMain_IUD @RequestMainId,@BusinessUnitId,
                                                                @RequestTypeId,
                                                                @EntryDate,@RequestDate,
                                                                @RequestDeadline,@UserID,
                                                                @Requester,@Status,
                                                                @SupplierCode,@RequestComment,
                                                                @OperatorComment,
                                                                @QualityRequired,@CurrencyCode,
                                                                @LogisticTotal,@Buyer,@Destination,
                                                                @NewRequestmainId = @NewRequestmainId OUTPUT,
                                                                @NewRequestNo = @NewRequestNo OUTPUT 
                                                                select @NewRequestmainId as NewRequestmainId,
                                                               
                                                                @NewRequestNo as NewRequestNo
";

                command.Parameters.AddWithValue(command, "@RequestMainId", model.RequestMainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@RequestTypeId", model.RequestTypeId);
                command.Parameters.AddWithValue(command, "@EntryDate", model.EntryDate);
                command.Parameters.AddWithValue(command, "@RequestDate", model.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", model.RequestDeadline);
                command.Parameters.AddWithValue(command, "@UserID", userId);
                command.Parameters.AddWithValue(command, "@Requester", model.Requester);
                command.Parameters.AddWithValue(command, "@Status", model.Status);
                command.Parameters.AddWithValue(command, "@SupplierCode", model.SupplierCode);
                command.Parameters.AddWithValue(command, "@RequestComment", model.RequestComment);
                command.Parameters.AddWithValue(command, "@OperatorComment", model.OperatorComment);
                command.Parameters.AddWithValue(command, "@QualityRequired", model.QualityRequired);
                command.Parameters.AddWithValue(command, "@Destination", model.Destination);
                command.Parameters.AddWithValue(command, "@CurrencyCode", model.CurrencyCode);
                command.Parameters.AddWithValue(command, "@Buyer", model.Buyer);
                command.Parameters.AddWithValue(command, "@LogisticTotal", model.LogisticTotal);

                command.Parameters.Add("@NewRequestmainId", SqlDbType.Int);
                command.Parameters["@NewRequestmainId"].Direction = ParameterDirection.Output;

                command.Parameters.Add("@NewRequestNo", SqlDbType.NVarChar, 50);
                command.Parameters["@NewRequestNo"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();
                int requestId = 0;
                string requestNo = "";
                if (reader.Read())
                {
                    requestId = reader.Get<int>("NewRequestmainId");
                    requestNo = reader.Get<string>("NewRequestNo");
                }

                return new RequestSaveResultModel { RequestMainId = requestId, RequestNo = requestNo };
            }
        }

        public async Task<List<RequestMainAll>> GetAllAsync(RequestMainGetModel requestMain)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainAll]", new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", string.Join(',', requestMain.ItemCodes));
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@ApproveStatus", string.Join(',', requestMain.ApproveStatus));
                command.Parameters.AddWithValue(command, "@Status", string.Join(',', requestMain.Status));

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMainAll> mainRequests = new List<RequestMainAll>();
                while (reader.Read())
                {
                    mainRequests.Add(reader.GetByEntityStructure<RequestMainAll>());
                }
                return mainRequests;
            }
        }

        public async Task<bool> UpdateBuyerAsync(string requestNo, string buyer)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF exec SP_SET_BUYER @RequestNO,@BUYER";

                command.Parameters.AddWithValue(command, "@RequestNO", requestNo.Trim());
                command.Parameters.AddWithValue(command, "@BUYER", buyer.Trim());
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RequestFollow>> RequestFollowUserLoadAsync(int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestFollow_Load @RequestMainId";

                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestFollow> followUsers = new List<RequestFollow>();
                while (reader.Read())
                {
                    followUsers.Add(reader.GetByEntityStructure<RequestFollow>());
                }
                return followUsers;
            }
        }

        public async Task<bool> RequestFollowSaveAsync(RequestFollowSaveModel saveModel)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RequestFollow_IUD @RequestFollowId,@UserId,
                                                                @RequestMainId"
                ;

                command.Parameters.AddWithValue(command, "@RequestFollowId", 0);
                command.Parameters.AddWithValue(command, "@UserId", saveModel.UserId);
                command.Parameters.AddWithValue(command, "@RequestMainId", saveModel.RequestMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> RequestFollowDeleteAsync(int requestFollowId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RequestFollow_IUD @RequestFollowId"
                ;

                command.Parameters.AddWithValue(command, "@RequestFollowId", requestFollowId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> RequestFollowCheckUserExistAsync(RequestFollowSaveModel saveModel)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"select dbo.SF_RequestFollowCheckExistUser (@UserId,@RequestMainId) AS Result ";

                command.Parameters.AddWithValue(command, "@UserId", saveModel.UserId);
                command.Parameters.AddWithValue(command, "@RequestMainId", saveModel.RequestMainId);

                using var reader = await command.ExecuteReaderAsync();
                bool res = false;
                string requestNo = "";
                if (reader.Read())
                {
                    res = reader.Get<bool>("Result");
                }

                return res;
            }
        }
    }
}

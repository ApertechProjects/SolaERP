using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static System.Runtime.CompilerServices.RuntimeHelpers;

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
                command.CommandText =
                    "EXEC SP_RequestMain_IUD @Id,NULL,NULL,NULL,NULL,NULL,@UserId, @NewRequestmainId = @NewRequestmainId OUTPUT, @NewRequestNo = @NewRequestNo OUTPUT select @NewRequestmainId as NewRequestmainId, @NewRequestNo as NewRequestNo";
                command.Parameters.AddWithValue(command, "@Id", Id);
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

        public async Task<bool> RequestMainChangeStatusAsync(int userId, int requestMainId, int approveStatus,
            string comment, int rejectReasonId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_RequestMainApprove @UserId,@Id,@ApprovalStatus,@Comment,@RejectReasonId";

                command.Parameters.AddWithValue(command, "@Id", requestMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApprovalStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@Comment", comment);
                command.Parameters.AddWithValue(command, "@RejectReasonId", rejectReasonId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RequestMainDraft>> GetMainRequestDraftsAsync(RequestMainDraftModel requestMain)
        {
            string itemCode = string.Join(',', requestMain.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainDrafts]",
                    new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", string.IsNullOrEmpty(itemCode) ? "%" : itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);

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
                command.CommandText = "SET NOCOUNT OFF EXEC SP_RequestSendToApprove @UserId,@Id";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Id", requestMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<RequestMain> GetRequestByRequestMainIdAsync(int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from Procurement.RequestMain where Id = @Id";

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
            string itemCode = string.Join(',', requestWFA.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainWFA]",
                    new ReplaceParams { ParamName = "APT", Value = requestWFA.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestWFA.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestWFA.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestWFA.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "-1" ? "%" : itemCode);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetWaitingForApprovalFromReader(reader));
                }

                return mainRequests;
            }
        }

        public async Task<List<RequestAmendment>> GetApproveAmendmentRequestsAsync(int userId,
            RequestApproveAmendmentModel requestParametersDto)
        {
            string itemCode = string.Join(',', requestParametersDto.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainApproveAmendment]",
                    new ReplaceParams { ParamName = "APT", Value = requestParametersDto.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestParametersDto.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestParametersDto.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestParametersDto.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "-1" ? "%" : itemCode);

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
                command.CommandText = "EXEC SP_RequestHeader_Load @Id,@UserId";
                command.Parameters.AddWithValue(command, "@Id", requestMainId);
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
                command.CommandText = "EXEC SP_RequestApprovalInfo @Id,@UserId";
                command.Parameters.AddWithValue(command, "@Id", requestMainId);
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
                Priority = reader.Get<int>("Priority")
            };
        }

        private RequestMain GetWaitingForApprovalFromReader(IDataReader reader)
        {
            return new()
            {
                RowNum = reader.Get<Int64>("RowNum"),
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                BuyerCode = reader.Get<string>("BuyerCode"),
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
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments")
            };
        }


        private RequestHeld GetHeldFromReader(IDataReader reader)
        {
            return new()
            {
                RowNum = reader.Get<Int64>("RowNum"),
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                BuyerCode = reader.Get<string>("BuyerCode"),
                BuyerName = reader.Get<string>("BuyerName"),
                RequestType = reader.Get<string>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments")
            };
        }

        private RequestCardMain GetRequestCardFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
                Destination = reader.Get<int>("Destination"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                AccountCode = reader.Get<string>("AccountCode"),
                AccountName = reader.Get<string>("AccountName"),
                Buyer = reader.Get<string>("Buyer"),
                PotentialVendor = reader.Get<string>("PotentialVendor"),
                Priority = reader.Get<int>("Priority"),
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
                KeyCode = reader.Get<string>("KeyCode"),
                Location = reader.Get<string>("Location")
            };
        }

        private RequestAmendment GetRequestAmendmentFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                BuyerCode = reader.Get<string>("BuyerCode"),
                BuyerName = reader.Get<string>("BuyerName"),
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
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments")
            };
        }

        private RequestMainDraft GetMainDraftFromReader(DbDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                RowNum = reader.Get<Int64>("RowNum"),
                RequestType = reader.Get<string>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                BuyerCode = reader.Get<string>("BuyerCode"),
                BuyerName = reader.Get<string>("BuyerName"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments")
            };
        }

        public async Task<RequestSaveResultModel> AddOrUpdateRequestAsync(int userId, RequestMainSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_RequestMain_IUD @Id,@BusinessUnitId,
                                                                @RequestTypeId,
                                                                @EntryDate,@RequestDate,
                                                                @RequestDeadline,@UserID,
                                                                @Requester,@Status,
                                                                @PotentialVendor,@RequestComment,
                                                                @OperatorComment,
                                                                @QualityRequired,@Currency,
                                                                @LogisticTotal,@Buyer,@Destination,
                                                                @Priority,@ApproveStageMainId,@Location,
                                                                @NewRequestmainId = @NewRequestmainId OUTPUT,
                                                                @NewRequestNo = @NewRequestNo OUTPUT 
                                                                select @NewRequestmainId as NewRequestmainId,
                                                               
                                                                @NewRequestNo as NewRequestNo
";

                command.Parameters.AddWithValue(command, "@Id", model.RequestMainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@RequestTypeId", model.RequestTypeId);
                command.Parameters.AddWithValue(command, "@EntryDate", model.EntryDate);
                command.Parameters.AddWithValue(command, "@RequestDate", model.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", model.RequestDeadline);
                command.Parameters.AddWithValue(command, "@UserID", userId);
                command.Parameters.AddWithValue(command, "@Requester", model.Requester);
                command.Parameters.AddWithValue(command, "@Status", model.Status);
                command.Parameters.AddWithValue(command, "@PotentialVendor", model.PotentialVendor);
                command.Parameters.AddWithValue(command, "@RequestComment", model.RequestComment);
                command.Parameters.AddWithValue(command, "@OperatorComment", model.OperatorComment);
                command.Parameters.AddWithValue(command, "@QualityRequired", model.QualityRequired);
                command.Parameters.AddWithValue(command, "@Currency", model.CurrencyCode);
                command.Parameters.AddWithValue(command, "@LogisticTotal", model.LogisticsTotal);
                command.Parameters.AddWithValue(command, "@Buyer", model.Buyer);
                command.Parameters.AddWithValue(command, "@Destination", model.Destination);
                command.Parameters.AddWithValue(command, "@Priority", model.Priority);
                command.Parameters.AddWithValue(command, "@ApproveStageMainId", model.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@Location", model.Location);

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
            string itemCode = string.Join(',', requestMain.ItemCode);
            string approveStatus = string.Join(',', requestMain.ApproveStatus);
            string status = string.Join(',', requestMain.Status);

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainAll]",
                    new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", string.IsNullOrEmpty(itemCode) ? "%" : itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@ApproveStatus",
                    approveStatus == "-1" ? "%" : approveStatus);
                command.Parameters.AddWithValue(command, "@Status", status == "-1" ? "%" : status);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestMainAll> mainRequests = new List<RequestMainAll>();

                while (reader.Read()) mainRequests.Add(reader.GetByEntityStructure<RequestMainAll>());
                return mainRequests;
            }
        }

        public async Task<bool> UpdateBuyerAsync(RequestSetBuyer setBuyer)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF exec SP_SET_BUYER @RequestNO,@BUYER";

                command.Parameters.AddWithValue(command, "@RequestNO", setBuyer.RequestNo);
                command.Parameters.AddWithValue(command, "@BUYER", setBuyer.Buyer);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RequestFollow>> RequestFollowUserLoadAsync(int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestFollow_Load @Id";

                command.Parameters.AddWithValue(command, "@Id", requestMainId);

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
                                                                @Id"
                    ;

                command.Parameters.AddWithValue(command, "@RequestFollowId", 0);
                command.Parameters.AddWithValue(command, "@UserId", saveModel.UserId);
                command.Parameters.AddWithValue(command, "@Id", saveModel.RequestMainId);

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
                command.CommandText = @"select dbo.SF_RequestFollowCheckExistUser (@UserId,@Id) AS Result ";

                command.Parameters.AddWithValue(command, "@UserId", saveModel.UserId);
                command.Parameters.AddWithValue(command, "@Id", saveModel.RequestMainId);

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

        public async Task<int> GetDefaultApprovalStage(string keyCode, int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = $"exec dbo.SP_RequestApproveStagesDefault @keyCode,@businessUnitId";
                command.Parameters.AddWithValue(command, "@keyCode", keyCode);
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();
                int mainId = 0;
                if (reader.Read())
                    mainId = reader.Get<int>("ApproveStageMainId");
                return mainId;
            }
        }

        public async Task<List<RequestCategory>> CategoryList()
        {
            List<RequestCategory> list = new List<RequestCategory>();
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = $"exec dbo.SP_RequestCatList";
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    list.Add(reader.GetByEntityStructure<RequestCategory>());
                return list;
            }
        }

        public async Task<List<RequestHeld>> GetHeldAsync(RequestWFAGetModel requestMain)
        {
            string itemCode = string.Join(',', requestMain.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainHeld]",
                    new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "-1" ? "%" : itemCode);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestHeld> mainRequests = new List<RequestHeld>();
                while (reader.Read())
                {
                    mainRequests.Add(GetHeldFromReader(reader));
                }

                return mainRequests;
            }
        }
    }
}
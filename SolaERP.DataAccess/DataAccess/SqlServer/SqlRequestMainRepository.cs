using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Mail;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using SolaERP.Application.Dtos.Request;
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

        public async Task<List<RequestMainDraft>> GetMainRequestDraftsAsync(RequestMainDraftModel requestMain,
            int userId)
        {
            string itemCode = string.Join(',', requestMain.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainDrafts]",
                    new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@UserId", userId);

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
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);

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
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);

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
                Requester = reader.Get<string>("Requester"),
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
                ApproveStatus = reader.Get<int>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                BuyerCode = reader.Get<string>("BuyerCode"),
                BuyerName = reader.Get<string>("BuyerName"),
                RequestType = reader.Get<string>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<string>("Requester"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments"),
                Sequence = reader.Get<int>("Sequence"),
                KeyCode = reader.Get<string>("KeyCode")
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
                Requester = reader.Get<string>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments"),
                Sequence = reader.Get<int>("Sequence")
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
                Requester = reader.Get<string>("Requester"),
                Status = reader.Get<int>("Status"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments"),
                RequestType = reader.Get<string>("RequestType"),
                RowNum = reader.Get<Int64>("RowNum"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName")
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
                Requester = reader.Get<string>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                ApproveStatusName = reader.Get<string>("ApproveStatusName"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName"),
                Priority = reader.Get<int>("Priority"),
                HasAttachments = reader.Get<bool>("HasAttachments"),
                KeyCode = reader.Get<string>("KeyCode")
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

        public async Task<List<RequestMainAll>> GetAllAsync(RequestMainGetModel requestMain, int userId)
        {
            string itemCode = string.Join(',', requestMain.ItemCode);
            string approveStatus = string.Join(',', requestMain.ApproveStatus);
            string status = string.Join(',', requestMain.Status);
            string requester = string.Join(',', requestMain.Requester);

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_RequestMainAll @BusinessUnitId, @ItemCode, @DateFrom, @DateTo, @ApproveStatus, @Status, @Requester, @UserId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApproveStatus", approveStatus == "-1" ? "%" : approveStatus);
                command.Parameters.AddWithValue(command, "@Status", status == "-1" ? "%" : status);
                command.Parameters.AddWithValue(command, "@Requester", requester == "-1" ? "%" : requester);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestMainAll> mainRequests = new List<RequestMainAll>();

                while (reader.Read()) mainRequests.Add(reader.GetByEntityStructure<RequestMainAll>());
                return mainRequests;
            }
        }

        public async Task<bool> UpdateBuyerAsync(RequestSetBuyer setBuyer, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF exec SP_SET_BUYER @RequestNO,@BUYER,@UserId,@BusinessUnitId";

                command.Parameters.AddWithValue(command, "@RequestNO", setBuyer.RequestNo);
                command.Parameters.AddWithValue(command, "@BUYER", setBuyer.Buyer);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", setBuyer.BusinessUnitId);

                await UpdateRequestDetailBuyerAsync(setBuyer.RequestDetails , userId , setBuyer.BusinessUnitId);
                
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

        public async Task<List<RequestCategory>> CategoryList(int businessUnitId, string keyCode)
        {
            List<RequestCategory> list = new List<RequestCategory>();
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = $"exec dbo.SP_CatList @businessUnitId, @keyCode";
                command.Parameters.AddWithValue(command, "@keyCode", keyCode);
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    list.Add(reader.GetByEntityStructure<RequestCategory>());
                return list;
            }
        }

        public async Task<List<RequestHeld>> GetHeldAsync(RequestWFAGetModel requestMain, int userId)
        {
            string itemCode = string.Join(',', requestMain.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainHeld]",
                    new ReplaceParams { ParamName = "APT", Value = requestMain.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestMain.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestMain.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestMain.DateTo);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestHeld> mainRequests = new List<RequestHeld>();
                while (reader.Read())
                {
                    mainRequests.Add(GetHeldFromReader(reader));
                }

                return mainRequests;
            }
        }

        public async Task<List<int>> GetDetailIds(int requestMainId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"select RequestDetailId from Procurement.RequestDetails where RequestMainId = @RequestMainId";
            command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            List<int> datas = new();
            while (await reader.ReadAsync())
            {
                datas.Add(reader.Get<int>("RequestDetailId"));
            }

            return datas;
        }

        public async Task<List<BuyersAssignment>> GetBuyersAssignment(RequestWFAGetModel model, int userId)
        {
            string itemCode = string.Join(',', model.ItemCode);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_RequestMainBuyersAssignment]",
                    new ReplaceParams { ParamName = "APT", Value = model.BusinessUnitCode });

                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode == "All" ? "%" : itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", model.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", model.DateTo);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@NotAssigneds", model.NotAssigneds ? 1 : 0);

                using var reader = await command.ExecuteReaderAsync();

                List<BuyersAssignment> mainDrafts = new List<BuyersAssignment>();
                while (reader.Read())
                    mainDrafts.Add(reader.GetByEntityStructure<BuyersAssignment>());
                return mainDrafts;
            }
        }


        public async Task<bool> Retrieve(int requestMainId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC  dbo.SP_RequestRetrieve @UserId, @RequestMainId";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

		public async Task<RequestMailDto> RequestEmailSendUsers(int requestMainId)
		{
			await using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText =
				@"exec dbo.SP_RequestMailSender @RequestMainId";
			command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

			await using DbDataReader reader = await command.ExecuteReaderAsync();
			RequestMailDto data = new();
			if (await reader.ReadAsync())
			{
                data.BusinessUnitName = reader.Get<string>("BusinessUnitName");
                data.BuyerEmail = reader.Get<string>("BuyerEmail");
                data.RequesterEmail = reader.Get<string>("RequesterEmail");
			}

			return data;
		}

		public async Task<UserList> RequesterMailInRequest(int requestMainId)
		{
			await using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText =
				@"exec dbo.SP_RequesterMailInRequest @RequestMainId";
			command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

			await using DbDataReader reader = await command.ExecuteReaderAsync();
			UserList data = new();
			if (await reader.ReadAsync())
			{
				data.Email = reader.Get<string>("Email");
				data.FullName = reader.Get<string>("FullName");
				data.UserId = reader.Get<int>("UserId");
				data.Language = reader.Get<string>("Language");
				data.RequestNo = reader.Get<string>("RequestNo");
			}

			return data;
		}

		public async Task<UserList> BuyerMailInRequest(int requestMainId)
		{
			await using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText =
				@"exec dbo.SP_BuyerMailInRequest @RequestMainId";
			command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

			await using DbDataReader reader = await command.ExecuteReaderAsync();
			UserList data = new();
			if (await reader.ReadAsync())
			{
				data.Email = reader.Get<string>("Email");
				data.FullName = reader.Get<string>("FullName");
				data.UserId = reader.Get<int>("UserId");
				data.Language = reader.Get<string>("Language");
				data.RequestNo = reader.Get<string>("RequestNo");
			}

			return data;
		}

		public async Task<string> GetRequestBusinessUnitName(int requestMainId)
		{
			await using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText =
				@"exec dbo.SP_GetRequestBusinessUnitName @RequestMainId";
			command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

			await using DbDataReader reader = await command.ExecuteReaderAsync();
            string result = null;
			if (await reader.ReadAsync())
			{
				result = reader.Get<string>("BusinessUnitName");
			}

			return result;
		}
        
        public async Task UpdateRequestDetailBuyerAsync(List<RequestDetailUpdateBuyerDto> buyers, int userId,
            int businessUnitId)
        {
            foreach (RequestDetailUpdateBuyerDto requestDetail in buyers)
            {
                using (var command = _unitOfWork.CreateCommand() as SqlCommand)
                {
                        command.CommandText = @"SET NOCOUNT OFF exec SP_RequestDetail_Update_Buyer @RequestDetailId,@Buyer,@UserId,@BusinessUnitId";

                        command.Parameters.AddWithValue(command, "@RequestDetailId", requestDetail.RequestDetailId);
                        command.Parameters.AddWithValue(command, "@Buyer", requestDetail.Buyer);
                        command.Parameters.AddWithValue(command, "@UserId", userId);
                        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                        
                        await command.ExecuteNonQueryAsync();
                }

            }
        }
        
        public async Task<List<WarehouseInfo>> GetWarehouseList(int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"exec SP_WarehouseList @BusinessUnitId";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();
            List<WarehouseInfo> warehouseInfo = new();

            while (await reader.ReadAsync())
            {
                 warehouseInfo.Add(reader.GetByEntityStructure<WarehouseInfo>());
            }

            return warehouseInfo;
        }
	}
}
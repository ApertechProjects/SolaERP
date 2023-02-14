using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Enums;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlRequestMainRepository : IRequestMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlRequestMainRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<RequestMain>> GetAllAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo, ApproveStatuses approveStatus, Status status)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMainAll @BusinessUnitId,@ItemCode,@DateFrom,@DateTo,@ApproveStatus,@Status";

                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
                command.Parameters.AddWithValue(command, "DateTo", dateTo);
                command.Parameters.AddWithValue(command, "@ApproveStatus", (byte)approveStatus);
                command.Parameters.AddWithValue(command, "@Status", (byte)status);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetRequestMainFromReader(reader));
                }
                if (mainRequests.Count == 0)
                    mainRequests.Add(new RequestMain { BusinessUnitId = 0 });
                return mainRequests;
            }
        }



        public async Task<int> DeleteAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "EXEC SP_RequestMain_IUD @RequestMainId";
                command.Parameters.AddWithValue(command, "@RequestMainId", Id);
                command.Parameters.Add("@NewRequestMainId", SqlDbType.Int);
                command.Parameters["@NewRequestMainId"].Direction = ParameterDirection.Output;

                await command.ExecuteNonQueryAsync();

                var returnValue = command.Parameters["@NewRequestMainId"].Value;
                return returnValue != DBNull.Value && returnValue != null ? Convert.ToInt32(returnValue) : 0;
            }
        }

        public async Task<int> AddOrUpdateAsync(RequestMainSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_RequestMain_IUD @RequestMainId,@BusinessUnitId,
                                                                @RequestNo,@RequestTypeId,
                                                                @EntryDate,@RequestDate,
                                                                @RequestDeadline,@UserID,
                                                                @Requester,@Status,
                                                                @SupplierCode,@RequestComment,
                                                                @OperatorComment,
                                                                @QualityRequired,@CurrencyCode,
                                                                @LogisticTotal";

                command.Parameters.AddWithValue(command, "@RequestMainId", model.RequestMainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@RequestNo", model.RequestNo);
                command.Parameters.AddWithValue(command, "@RequestTypeId", model.RequestTypeId);
                command.Parameters.AddWithValue(command, "@EntryDate", model.EntryDate);
                command.Parameters.AddWithValue(command, "@RequestDate", model.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", model.RequestDeadline);
                command.Parameters.AddWithValue(command, "@UserID", model.UserId);
                command.Parameters.AddWithValue(command, "@Requester", model.Requester);
                command.Parameters.AddWithValue(command, "@Status", model.Status);
                command.Parameters.AddWithValue(command, "@SupplierCode", model.CurrencyCode);
                command.Parameters.AddWithValue(command, "@RequestComment", model.RequestComment);
                command.Parameters.AddWithValue(command, "@OperatorComment", model.OperatorComment);
                command.Parameters.AddWithValue(command, "@QualityRequired", model.QualityRequired);
                command.Parameters.AddWithValue(command, "@CurrencyCode", model.CurrencyCode);
                command.Parameters.AddWithValue(command, "@LogisticTotal", model.LogisticTotal);


                var newRequestmainId = new SqlParameter("@NewRequestMainId", SqlDbType.Int);
                newRequestmainId.Direction = ParameterDirection.Output;
                command.Parameters.Add(newRequestmainId);

                await command.ExecuteNonQueryAsync();
                var returnValue = command.Parameters["@NewRequestMainId"].Value;

                return returnValue != DBNull.Value && returnValue != null ? Convert.ToInt32(returnValue) : 0;
            }
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

        public async Task<bool> ChangeRequestStatusAsync(RequestChangeStatusParametersDto changeStatusParametersDto)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMain_IUD @RequestMainId,@RequestDetailId,@UserId,@Sequence,@Status,@ApproveStatus,@Comment";

                command.Parameters.AddWithValue(command, "@RequestMainId", changeStatusParametersDto.RequestMainId);
                command.Parameters.AddWithValue(command, "@RequestDetailId", changeStatusParametersDto.RequestDetailId);
                command.Parameters.AddWithValue(command, "@UserId", changeStatusParametersDto.UserId);
                command.Parameters.AddWithValue(command, "@Sequence", changeStatusParametersDto.Sequence);
                command.Parameters.AddWithValue(command, "@Status", changeStatusParametersDto.Status);
                command.Parameters.AddWithValue(command, "@Comment", changeStatusParametersDto.Comment);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RequestMainDraft>> GetAllMainRequestDraftsAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMainDrafts @BusinessUnitId,@ItemCode,@DateFrom,@DateTo";

                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
                command.Parameters.AddWithValue(command, "DateTo", dateTo);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMainDraft> mainDrafts = new List<RequestMainDraft>();
                while (reader.Read())
                    mainDrafts.Add(GetMainDraftFromReader(reader));
                if (mainDrafts.Count == 0)
                    mainDrafts.Add(new RequestMainDraft { Status = 0 });
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

        public async Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, int businessUnitId, DateTime dateFrom, DateTime dateTo, string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMainWFA @UserId,@BusinessUnitId,@DateFrom,@DateTo,@ItemCode";

                command.Parameters.AddWithValue(command, "@UserId", itemCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", dateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetWaitingForApprovalFromReader(reader));
                }
                if (mainRequests.Count == 0)
                    mainRequests.Add(new RequestMain { BusinessUnitId = 0 });
                return mainRequests;
            }
        }

        public async Task<List<RequestAmendment>> GetApproveAmendmentRequestsAsync(int userId, RequestApproveAmendmentGetModel requestParametersDto)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_RequestMainApproveAmendment @UserId,@BusinessUnitId,@DateFrom,@DateTo,@ItemCode";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", requestParametersDto.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", requestParametersDto.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", requestParametersDto.DateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", requestParametersDto.ItemCode);

                List<RequestAmendment> mainRequestsForAmendment = new();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    mainRequestsForAmendment.Add(GetRequestAmendmentFromReader(reader));
                }

                if (mainRequestsForAmendment.Count == 0)
                    mainRequestsForAmendment.Add(new() { BusinessUnitId = 1, RequestComment = "Tessst", Requester = "ShaSha", RequestMainId = 1, Status = 1 });

                return mainRequestsForAmendment;
            }
        }


        public async Task<RequestMain> GetRequesMainHeaderAsync(int requestMainId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestHeader_Load @RequestMainId,@UserId";
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                RequestMain result = null;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read()) result = GetWaitingForApprovalFromReader(reader);

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
                RequsetDeadline = reader.Get<DateTime>("RequestDeadline"),
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
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RequestTypeId = reader.Get<int>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequsetDeadline = reader.Get<DateTime>("RequestDeadline"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
            };
        }

        private RequestMain GetFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                Status = reader.Get<int>("Status"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RowNum = reader.Get<int>("RowNum"),
                RequestTypeId = reader.Get<int>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequsetDeadline = reader.Get<DateTime>("RequestDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
            };
        }

        private RequestAmendment GetRequestAmendmentFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RequetsTypeId = reader.Get<int>("RequetsTypeId"),
                RequestNo = reader.Get<int>("RequestNo"),
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
                LogisticTotal = reader.Get<int>("LogisticTotal"),

            };
        }

        private RequestMainDraft GetMainDraftFromReader(DbDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitCode = reader.Get<string>("BusinessunitCode"),
                RowNum = reader.Get<int>("RowNum"),
                RequestType = reader.Get<string>("RequestType"),
                RequetsNo = reader.Get<int>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntrDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                Requester = reader.Get<string>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualitiyRequired"),
                ApproveStatus = reader.Get<string>("ApproveStatus")
            };
        }


        private RequestMain GetRequestMainFromReader(DbDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                Status = reader.Get<int>("Status"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RowNum = reader.Get<int>("RowNum"),
                RequestTypeId = reader.Get<int>("RequestType"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequsetDeadline = reader.Get<DateTime>("RequestDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                Requester = reader.Get<int>("Requester"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
            };
        }


    }

}

using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Enums;
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
                command.Parameters.AddWithValue(command, "@ApproveStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@Status", status);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetAllFromReader(reader));
                }
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


        public async Task<int> AddOrUpdateAsync(RequestMain entity)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_RequestMain_IUD @BusinessUnitId,
                                                                @RequestNo,@RequestTypeId,
                                                                @EntryDate,@RequestDate,
                                                                @RequestDeadline,@UserID,
                                                                @Requester,@Status,
                                                                @SupplierCode,@RequestComment,
                                                                @OperatorComment,
                                                                @QualityRequired,@CurrencyCode,
                                                                @LogisticTotal";

                command.Parameters.AddWithValue(command, "@BusinessUnitId", entity.RequestMainId);
                command.Parameters.AddWithValue(command, "@RequestNo", entity.RequestNo);
                command.Parameters.AddWithValue(command, "@RequestTypeId", entity.RequestTypeId);
                command.Parameters.AddWithValue(command, "@EntryDate", entity.EntryDate);
                command.Parameters.AddWithValue(command, "@RequestDate", entity.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", entity.RequsetDeadline);
                command.Parameters.AddWithValue(command, "@UserID", entity.UserId);
                command.Parameters.AddWithValue(command, "@Requester", entity.Requester);
                command.Parameters.AddWithValue(command, "@Status", entity.Status);
                command.Parameters.AddWithValue(command, "@SupplierCode", entity.CurrencyCode);
                command.Parameters.AddWithValue(command, "@RequestComment", entity.RequestComment);
                command.Parameters.AddWithValue(command, "@OperatorComment", entity.OperatorComment);
                command.Parameters.AddWithValue(command, "@QualityRequired", entity.QualityRequired);
                command.Parameters.AddWithValue(command, "@CurrencyCode", entity.CurrencyCode);
                command.Parameters.AddWithValue(command, "@LogisticTotal", entity.LogisticTotal);

                command.Parameters.Add("@NewRequestMainId", SqlDbType.Int);
                command.Parameters["@NewRequestMainId"].Direction = ParameterDirection.Output;

                await command.ExecuteNonQueryAsync();
                var returnValue = command.Parameters["@NewRequestMainId"].Value;

                return returnValue != DBNull.Value && returnValue != null ? Convert.ToInt32(returnValue) : 0;
            }
        }

        private RequestMain GetAllFromReader(IDataReader reader)
        {
            return new RequestMain
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

        private RequestMain GetWaitingForApprovalFromReader(IDataReader reader)
        {
            return new RequestMain
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
                LogisticTotal = reader.Get<decimal>("LogisticTotal"),
            };
        }

        public async Task<List<RequestMain>> GetWaitingForApprovalsAsync(int userId, int businessUnitId, DateTime dateFrom, DateTime dateTo, string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestMainWFA @UserId,@BusinessUnitId,@DateFrom,@DateTo,@ItemCode";

                command.Parameters.AddWithValue(command, "@UserId", itemCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
                command.Parameters.AddWithValue(command, "DateTo", dateTo);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);

                using var reader = await command.ExecuteReaderAsync();

                List<RequestMain> mainRequests = new List<RequestMain>();
                while (reader.Read())
                {
                    mainRequests.Add(GetWaitingForApprovalFromReader(reader));
                }
                return mainRequests;
            }
        }
    }
}

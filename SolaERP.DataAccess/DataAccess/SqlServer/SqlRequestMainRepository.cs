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
                    mainRequests.Add(GetFromReader(reader));
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

        private RequestMain GetFromReader(IDataReader reader)
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

        private RequestTypes GetRequestTypesFromReader(IDataReader reader)
        {
            return new RequestTypes
            {
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestType = reader.Get<string>("RequestType"),
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId")
            };
        }

        public async Task<List<RequestTypes>> GetRequestTypesByBusinessUnitId(int businessUnitId)
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
    }
}


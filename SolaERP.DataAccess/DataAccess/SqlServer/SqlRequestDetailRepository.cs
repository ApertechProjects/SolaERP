using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlRequestDetailRepository : IRequestDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlRequestDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(RequestDetail entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"Exec SP_RequestDetails_IUD  @RequestDetailId,  
                                                                    @RequestMainId, 
                                                                    @LineNo, 
                                                                    @RequestDate, 
                                                                    @RequestDeadline, 
                                                                    @RequestedDate, 
                                                                    @ItemCodes, 
                                                                    @Quantity, 
                                                                    @UOM, 
                                                                    @Description, 
                                                                    @Location, 
                                                                    @Buyer, 
                                                                    @AvailableQuantity, 
                                                                    @QuantityFromStock, 
                                                                    @OriginalQuantity, 
                                                                    @TotalBudget, 
                                                                    @RemainingBudget, 
                                                                    @Amount, 
                                                                    @ConnectedOrderReference, 
                                                                    @ConnectedOrderLineNo, 
                                                                    @AccountCode, 
                                                                    @AnalysisCode1Id, 
                                                                    @AnalysisCode2Id, 
                                                                    @AnalysisCode3Id,  
                                                                    @AnalysisCode4Id, 
                                                                    @AnalysisCode5Id,
                                                                    @AnalysisCode6Id, 
                                                                    @AnalysisCode7Id, 
                                                                    @AnalysisCode8Id, 
                                                                    @AnalysisCode9Id, 
                                                                    @AnalysisCode10Id,
                                                                    @NewRequestDetailsId";

                command.Parameters.AddWithValue(command, "@RequestDetailId", entity.RequestDetailId);
                command.Parameters.AddWithValue(command, "@RequestMainId", entity.RequestMainId);
                command.Parameters.AddWithValue(command, "@LineNo", entity.LineNo.Trim());
                command.Parameters.AddWithValue(command, "@RequestDate", entity.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", entity.RequestDeadline);
                command.Parameters.AddWithValue(command, "@RequestedDate", entity.RequestedDate);
                command.Parameters.AddWithValue(command, "@ItemCodes", entity.ItemCode.Trim());
                command.Parameters.AddWithValue(command, "@Quantity", entity.Quantity);
                command.Parameters.AddWithValue(command, "@UOM", entity.UOM.Trim());
                command.Parameters.AddWithValue(command, "@Description", entity.Description.Trim());
                command.Parameters.AddWithValue(command, "@Location", entity.Location.Trim());
                command.Parameters.AddWithValue(command, "@Buyer", entity.Buyer.Trim());
                command.Parameters.AddWithValue(command, "@AvailableQuantity", entity.AvailableQuantity);
                command.Parameters.AddWithValue(command, "@QuantityFromStock", entity.QuantityFromStock);
                command.Parameters.AddWithValue(command, "@OriginalQuantity", entity.OriginalQuantity);
                command.Parameters.AddWithValue(command, "@TotalBudget", entity.TotalBudget);
                command.Parameters.AddWithValue(command, "@RemainingBudget", entity.RemainingBudget);
                command.Parameters.AddWithValue(command, "@Amount", entity.Amount);
                command.Parameters.AddWithValue(command, "@ConnectedOrderReference", entity.ConnectedOrderReference);
                command.Parameters.AddWithValue(command, "@ConnectedOrderLineNo", entity.ConnectedOrderLineNo);
                command.Parameters.AddWithValue(command, "@AccountCode", entity.AccountCode.Trim());
                command.Parameters.AddWithValue(command, "@AnalysisCode1Id", entity.AnalysisCode1Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode2Id", entity.AnalysisCode2Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode3Id", entity.AnalysisCode3Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode4Id", entity.AnalysisCode4Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode5Id", entity.AnalysisCode5Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode6Id", entity.AnalysisCode6Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode7Id", entity.AnalysisCode7Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode8Id", entity.AnalysisCode8Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode9Id", entity.AnalysisCode9Id);
                command.Parameters.AddWithValue(command, "@AnalysisCode10Id", entity.AnalysisCode10Id);
                command.Parameters.AddOutPutParameter(command, "@NewRequestDetailsId");

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }


        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"exec SP_RequestDetails_IUD @RequestDetailId";
                command.Parameters.AddWithValue(command, "@RequestDetailId", Id);
                var value = await command.ExecuteNonQueryAsync();

                return value > 0;
            }
        }

        public async Task UpdateAsync(RequestDetail entity)
        {
            await AddAsync(entity);
        }

        public async Task<List<RequestCardDetail>> GetRequestDetailsByMainIdAsync(int requestMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestDetails_Load @RequestMainId";
                command.Parameters.AddWithValue(command, "@RequestMainId", requestMainId);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestCardDetail> resultList = new();

                while (await reader.ReadAsync()) resultList.Add(reader.GetByEntityStructure<RequestCardDetail>());
                if (resultList.Count == 0)
                    resultList.Add(new RequestCardDetail { Amount = 0 });
                return resultList;
            }
        }

        public async Task<List<RequestDetailApprovalInfo>> GetDetailApprovalInfoAsync(int requestDetailId)
        {
            List<RequestDetailApprovalInfo> result = new List<RequestDetailApprovalInfo>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestApprovals @RequestDetailId";
                command.Parameters.AddWithValue(command, "@RequestDetailId", requestDetailId);
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) result.Add(reader.GetByEntityStructure<RequestDetailApprovalInfo>());
                return result;
            }
        }

        public async Task<bool> RequestDetailChangeStatusAsync(int requestDetailId, int userId, int approveStatusId, string comment, int sequence)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_RequestApprove @RequestDetailId,@UserId,@ApproveStatusId,@Comment,@Sequence";

                command.Parameters.AddWithValue(command, "@RequestDetailId", requestDetailId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApproveStatusId", approveStatusId);
                command.Parameters.AddWithValue(command, "@Comment", comment);
                command.Parameters.AddWithValue(command, "@Sequence", sequence);

                return await command.ExecuteNonQueryAsync() > 0;
            }

        }


        public Task<List<RequestDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RequestDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}

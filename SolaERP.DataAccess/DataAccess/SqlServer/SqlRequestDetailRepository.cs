using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
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
                command.CommandText = @"Exec SP_RequestDetails_IU  @RequestDetailId,  
                                                                    @Id, 
                                                                    @LineNo, 
                                                                    @RequestDate, 
                                                                    @RequestDeadline, 
                                                                    @RequestedDate, 
                                                                    @ItemCode, 
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
                                                                    @Condition,
                                                                    @Priority,
                                                                    @ManualUP,
                                                                    @AlternativeItem,
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
                                                                    @Catid,
                                                                    @NewRequestDetailsId";

                command.Parameters.AddWithValue(command, "@RequestDetailId", entity.RequestDetailId);
                command.Parameters.AddWithValue(command, "@Id", entity.RequestMainId);
                command.Parameters.AddWithValue(command, "@LineNo", entity.LineNo.Trim());
                command.Parameters.AddWithValue(command, "@RequestDate", entity.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", entity.RequestDeadline);
                command.Parameters.AddWithValue(command, "@RequestedDate", entity.RequestedDate);
                command.Parameters.AddWithValue(command, "@ItemCode", entity.ItemCode);
                command.Parameters.AddWithValue(command, "@Quantity", entity.Quantity);
                command.Parameters.AddWithValue(command, "@UOM", entity.UOM);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@Location", entity.Location);
                command.Parameters.AddWithValue(command, "@Buyer", entity.Buyer);
                command.Parameters.AddWithValue(command, "@AvailableQuantity", entity.AvailableInMainStock);
                command.Parameters.AddWithValue(command, "@QuantityFromStock", entity.QuantityFromStock);
                command.Parameters.AddWithValue(command, "@OriginalQuantity", entity.OriginalQuantity);
                command.Parameters.AddWithValue(command, "@TotalBudget", entity.TotalBudget);
                command.Parameters.AddWithValue(command, "@RemainingBudget", entity.RemainingBudget);
                command.Parameters.AddWithValue(command, "@Amount", entity.Amount);
                command.Parameters.AddWithValue(command, "@ConnectedOrderReference", entity.ConnectedOrderReference);
                command.Parameters.AddWithValue(command, "@ConnectedOrderLineNo", entity.ConnectedOrderLineNo);
                command.Parameters.AddWithValue(command, "@AccountCode", entity.AccountCode);
                command.Parameters.AddWithValue(command, "@Condition", entity.Condition);
                command.Parameters.AddWithValue(command, "@Priority", entity.Priority);
                command.Parameters.AddWithValue(command, "@ManualUP", entity.ManualUP);
                command.Parameters.AddWithValue(command, "@AlternativeItem", entity.AlternativeItem);
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
                command.Parameters.AddWithValue(command, "@Catid", entity.Catid);
                command.Parameters.AddOutPutParameter(command, "@NewRequestDetailsId");

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }


        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $@"SET NOCOUNT OFF Exec SP_RequestDetails_D  @RequestDetailId";

                command.Parameters.AddWithValue(command, "@RequestDetailId", Id);

                return await command.ExecuteNonQueryAsync() > 0;
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
                command.CommandText = "EXEC SP_RequestDetails_Load @Id";
                command.Parameters.AddWithValue(command, "@Id", requestMainId);

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

        public async Task<bool> RequestDetailChangeStatusAsync(int requestDetailId, int userId, int approveStatusId, string comment, int sequence, int rejectReasonId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_RequestApprove @RequestDetailId,@UserId,@ApproveStatusId,@Comment,@Sequence,@RejectReasonId";

                command.Parameters.AddWithValue(command, "@RequestDetailId", requestDetailId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApproveStatusId", approveStatusId);
                command.Parameters.AddWithValue(command, "@Comment", comment);
                command.Parameters.AddWithValue(command, "@Sequence", sequence);
                command.Parameters.AddWithValue(command, "@RejectReasonId", rejectReasonId);

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

        public async Task<List<RequestCardAnalysis>> GetAnalysis(int requestMainId)
        {
            List<RequestCardAnalysis> result = new List<RequestCardAnalysis>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RequestDetailsAnalysis_Load @Id";
                command.Parameters.AddWithValue(command, "@Id", requestMainId);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) result.Add(reader.GetByEntityStructure<RequestCardAnalysis>());
                return result;
            }
        }
    }
}

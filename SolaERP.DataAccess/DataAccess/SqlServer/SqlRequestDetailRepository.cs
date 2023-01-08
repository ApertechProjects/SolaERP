using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
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

        public Task<bool> AddAsync(RequestDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddOrUpdateAsync(RequestDetail entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"Exec SP_RequestDetails_IUD  @RequestDetailId,  
                                                                    @RequestMainId, 
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
                                                                    @AnalysisCode1Id, 
                                                                    @AnalysisCode2Id, 
                                                                    @AnalysisCode3Id,  
                                                                    @AnalysisCode4Id, 
                                                                    @AnalysisCode5Id,
                                                                    @AnalysisCode6Id, 
                                                                    @AnalysisCode7Id, 
                                                                    @AnalysisCode8Id, 
                                                                    @AnalysisCode9Id, 
                                                                    @AnalysisCode10Id";

                command.Parameters.AddWithValue(command, "@RequestDetailId", entity.RequestDetailId);
                command.Parameters.AddWithValue(command, "@RequestMainId", entity.RequestMainId);
                command.Parameters.AddWithValue(command, "@LineNo", entity.LineNo);
                command.Parameters.AddWithValue(command, "@RequestDate", entity.RequestDate);
                command.Parameters.AddWithValue(command, "@RequestDeadline", entity.RequestDeadline);
                command.Parameters.AddWithValue(command, "@RequestedDate", entity.RequestedDate);
                command.Parameters.AddWithValue(command, "@ItemCode", entity.ItemCode);
                command.Parameters.AddWithValue(command, "@Quantity", entity.Quantity);
                command.Parameters.AddWithValue(command, "@UOM", entity.UOM);
                command.Parameters.AddWithValue(command, "@Description", entity.Description);
                command.Parameters.AddWithValue(command, "@Location", entity.Location);
                command.Parameters.AddWithValue(command, "@Buyer", entity.Buyer);
                command.Parameters.AddWithValue(command, "@AvailableQuantity", entity.AvailableQuantity);
                command.Parameters.AddWithValue(command, "@QuantityFromStock", entity.QuantityFromStock);
                command.Parameters.AddWithValue(command, "@OriginalQuantity", entity.OriginalQuantity);
                command.Parameters.AddWithValue(command, "@TotalBudget", entity.TotalBudget);
                command.Parameters.AddWithValue(command, "@RemainingBudget", entity.RemainingBudget);
                command.Parameters.AddWithValue(command, "@Amount", entity.Amount);
                command.Parameters.AddWithValue(command, "@ConnectedOrderReference", entity.ConnectedOrderReference);
                command.Parameters.AddWithValue(command, "@ConnectedOrderLineNo", entity.ConnectedOrderLineNo);
                command.Parameters.AddWithValue(command, "@AccountCode", entity.AccountCode);
                command.Parameters.AddWithValue(command, "@AnalysisCode1Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode2Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode3Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode4Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode5Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode6Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode7Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode8Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode9Id", null);
                command.Parameters.AddWithValue(command, "@AnalysisCode10Id", null);

                return await command.ExecuteNonQueryAsync();
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

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRequestDetailAsync(int id)
        {
            using (var commad = _unitOfWork.CreateCommand())
            {
                commad.CommandText = $"exec SP_RequestDetails_IUD @RequestDetailId";
                IDbDataParameter dbDataParameter = commad.CreateParameter();
                dbDataParameter.ParameterName = "@RequestDetailId";
                dbDataParameter.Value = id;
                commad.Parameters.Add(dbDataParameter);
                var value = commad.ExecuteNonQuery();
                return value == 0 || value == -1 ? false : true;
            }
        }

        public void Update(RequestDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}

using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGeneralRepository : IGeneralRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlGeneralRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BusinessCategory>> BusinessCategories()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SELECT BusinessCategoryId, 
                    BusinessCategoryName,
                    BusinessSectorId,
                    BusinessCategoryCode
                    FROM Register.BusinessCategory";

                List<BusinessCategory> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<BusinessCategory>());

                return resultList;
            }
        }

        public async Task<List<Status>> GetStatus()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_Status_List";
                using var reader = await command.ExecuteReaderAsync();

                List<Status> status = new List<Status>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Status>());
                }

                return status;
            }
        }

        public async Task<List<RejectReason>> RejectReasons()
        {
            List<RejectReason> rejectReasons = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from VW_RejectReasons";
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    rejectReasons.Add(reader.GetByEntityStructure<RejectReason>());
                }

                return rejectReasons;
            }
        }

        public async Task<List<RejectReason>> RejectReasonsForInvoice()
        {
            List<RejectReason> rejectReasons = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from VW_RejectReasons where RejectReasonID IN(1,2)";
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    rejectReasons.Add(reader.GetByEntityStructure<RejectReason>());
                }

                return rejectReasons;
            }
        }

        public async Task<List<ConvRateDto>> GetConvRateList(int businessUnitId)
        {
            List<ConvRateDto> resultList = new();
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC dbo.SP_ConvRate @BusinessUnitId";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultList.Add(new ConvRateDto
                {
                    EffFromDateTime = reader.Get<DateTime>("EFF_FROM_DATETIME"),
                    EffToDateTime = reader.Get<DateTime>("EFF_TO_DATETIME"),
                    CurrCodeFrom = reader.Get<string>("CURR_CODE_FROM"),
                    CurrCodeTo = reader.Get<string>("CURR_CODE_TO"),
                    ConvRate = reader.Get<decimal>("CONV_RATE"),
                    MultiplyDivide = reader.Get<short>("MULTIPLY_DIVIDE")
                });
            }

            return resultList;
        }
    }
}
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;
using System.Data.Common;
using SolaERP.Application.Dtos.Entitlement;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlEntitlementRepository : IEntitlementRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public SqlEntitlementRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> SaveEntitlementIUD(EntitlementUIDDto dto)
    {
        using (var command = _unitOfWork.CreateCommand() as DbCommand)
        {
            command.CommandText =
                @"SET NOCOUNT OFF Exec SP_EntitlementRegister_IUD  @EntitlementRegisterId,@BusinessUnitId,@Period,@Date,@Opex,@CorrectionToPriorPeriodsOpex,@Capex,@CorrectionToPriorPeriodsCapex,@UserId";
            command.Parameters.AddWithValue(command, "@EntitlementRegisterId", dto.EntitlementRegisterId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@Period", dto.Period);
            command.Parameters.AddWithValue(command, "@Date", dto.Date);
            command.Parameters.AddWithValue(command, "@Opex", dto.Opex);
            command.Parameters.AddWithValue(command, "@CorrectionToPriorPeriodsOpex", dto.CorrectionToPriorPeriodsOpex);
            command.Parameters.AddWithValue(command, "@Capex", dto.Capex);
            command.Parameters.AddWithValue(command, "@CorrectionToPriorPeriodsCapex",
                dto.CorrectionToPriorPeriodsCapex);
            command.Parameters.AddWithValue(command, "@UserId", dto.UserId);

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }

    public Task<bool> SaveEntitlementIUDv(EntitlementUIDDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<EntitlementListDto>> GetEntitlementsList(int businessUnitId, int periodFrom,
        int periodTo)
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_EntitlementRegisterLoad @BusinessUnitId, @PeriodFrom, @PeriodTo";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
        command.Parameters.AddWithValue(command, "@PeriodFrom", periodFrom);
        command.Parameters.AddWithValue(command, "@PeriodTo", periodTo);

        var data = new List<EntitlementListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new EntitlementListDto
            {
                EntitlementRegisterId = reader.Get<int>("EntitlementRegisterId"),
                Period = reader.Get<int>("Period"),
                Date = reader.Get<DateTime>("Date"),
                Opex = reader.Get<decimal>("Opex"),
                CorrectionToPriorPeriodsOpex = reader.Get<decimal>("CorrectionToPriorPeriodsOpex"),
                Capex = reader.Get<decimal>("Capex"),
                CorrectionToPriorPeriodsCapex = reader.Get<decimal>("CorrectionToPriorPeriodsCapex"),
                CreatedBy = reader.Get<int>("CreatedBy"),
                CreatedName = reader.Get<string>("CreatedName"),
                CreatedDate = reader.Get<DateTime>("CreatedDate"),
            });
        }

        return data;
    }
}
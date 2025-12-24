using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.WellRepair;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlWellRepairRepository : IWellRepairRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SqlWellRepairRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<WellRepairListDto>> GetWellRepairList()
    {
        using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_WellRepairList";

        var data = new List<WellRepairListDto>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            data.Add(new WellRepairListDto
            {
                Status = reader.Get<string>("Status"),
                CreatedDate = reader.Get<DateTime>("CreatedDate"),
                CreatedBy = reader.Get<string>("CreatedBy"),
                RepairCode = reader.Get<string>("RepairCode"),
                RepairNameAz = reader.Get<string>("NameAz"),
                RepairNameEng = reader.Get<string>("NameEng"),
                WellRepairId = reader.Get<string>("WellRepairId")
            });
        }
        return data;
    }
}














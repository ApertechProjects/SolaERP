using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Order;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlOrderRepository : IOrderRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SqlOrderRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<OrderTypeLoadDto>> GetAllOrderTypesByBusinessIdAsync(int businessUnitId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderTypeList @BusinessUnitId";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderTypeLoadDto> data = new();
        while (await reader.ReadAsync())
            data.Add(new OrderTypeLoadDto
            {
                OrderType = reader.Get<string>("Ordertype"),
                OrderTypeId = reader.Get<int>("OrderTypeId"),
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId")
            });

        return data;
    }
}
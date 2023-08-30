using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Order;
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

    public async Task<List<OrderAllDto>> GetAllAsync(OrderFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderAll @BusinessUnitId, @ItemCode, @OrderTypeId, 
    @UserId, @Status, @ApproveStatus, @DateFrom, @DateTo";


        string orderTypeIdString = string.Join(",", dto.OrderTypeId.Select(x => x.ToString()).ToList());
        string approveStatusString = string.Join(",", dto.ApproveStatus.Select(x => x.ToString()).ToList());
        string statusString = string.Join(",", dto.Status.Select(x => x.ToString()).ToList());
        
        if (dto.ItemCode[0] == "All" || dto.ItemCode.Length == 0)
        {
            dto.ItemCode = new[] { "-1" };
        }
        
        string itemCodeString = string.Join(",", dto.ItemCode.Select(x => x.ToString()).ToList());

        command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
        command.Parameters.AddWithValue(command, "@ItemCode", itemCodeString);
        command.Parameters.AddWithValue(command, "@OrderTypeId", orderTypeIdString);
        command.Parameters.AddWithValue(command, "@UserId", userId);
        command.Parameters.AddWithValue(command, "@Status", statusString);
        command.Parameters.AddWithValue(command, "@ApproveStatus", approveStatusString);
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderAllDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(new OrderAllDto
            {
                OrderType = reader.Get<string>("Ordertype"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                Comment = reader.Get<string>("Comment"),
                Currency = reader.Get<string>("Currency"),
                Sequence = reader.Get<int>("Sequence"),
                Status = reader.Get<string>("Status"),
                BidNo = reader.Get<string>("BidNo"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                EnteredDate = reader.Get<DateTime>("EnteredDate"),
                OrderNo = reader.Get<string>("OrderNo"),
                VendorName = reader.Get<string>("VendorName"),
                ApproveStageDetailsName = reader.Get<string>("ApproveStageDetailsName"),
                RFQNo = reader.Get<string>("RFQNo")
            });
        }

        return data;
    }

    public async Task<List<OrderAllDto>> GetChangeApprovalAsync(OrderChangeApprovalFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderChangeApprovals @BusinessUnitId, @ItemCode, @OrderTypeId, @UserId";

        string orderTypeIdString = string.Join(",", dto.OrderTypeId.Select(x => x.ToString()).ToList());

        if (dto.ItemCode[0] == "All" || dto.ItemCode.Length == 0)
        {
            dto.ItemCode = new[] { "-1" };
        }
        
        string itemCodeString = string.Join(",", dto.ItemCode.Select(x => x.ToString()).ToList());

        command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
        command.Parameters.AddWithValue(command, "@ItemCode", itemCodeString);
        command.Parameters.AddWithValue(command, "@OrderTypeId", orderTypeIdString);
        command.Parameters.AddWithValue(command, "@UserId", userId);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderAllDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(new OrderAllDto
            {
                OrderType = reader.Get<string>("Ordertype"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                Comment = reader.Get<string>("Comment"),
                Currency = reader.Get<string>("Currency"),
                Sequence = reader.Get<int>("Sequence"),
                Status = reader.Get<string>("Status"),
                BidNo = reader.Get<string>("BidNo"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                EnteredDate = reader.Get<DateTime>("EnteredDate"),
                OrderNo = reader.Get<string>("OrderNo"),
                VendorName = reader.Get<string>("VendorName"),
                ApproveStageDetailsName = reader.Get<string>("ApproveStageDetailsName"),
                RFQNo = reader.Get<string>("RFQNo")
            });
        }

        return data;
    }
}
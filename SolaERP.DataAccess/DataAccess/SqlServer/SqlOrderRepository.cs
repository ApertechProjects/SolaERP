using System.Data;
using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Vendors;
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
            data.Add(MapFromReader(reader));
        }

        return data;
    }

    public async Task<List<OrderAllDto>> GetWFAAsync(OrderWFAFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderWFA @BusinessUnitId, @ItemCode, @OrderTypeId, @UserId";

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
            data.Add(MapFromReader(reader));
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
            data.Add(MapFromReader(reader));
        }

        return data;
    }

    public async Task<List<OrderAllDto>> GetHeldAsync(OrderHeldFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText =
            @"EXEC dbo.SP_OrderHeld @BusinessUnitId, @ItemCode, @OrderTypeId, @UserId, @DateFrom, @DateTo";

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
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderAllDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReader(reader));
        }

        return data;
    }

    public async Task<List<OrderAllDto>> GetRejectedAsync(OrderRejectedFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText =
            @"EXEC dbo.SP_OrderRejected @BusinessUnitId, @ItemCode, @OrderTypeId, @UserId, @DateFrom, @DateTo";

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
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderAllDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReader(reader));
        }

        return data;
    }

    public async Task<List<OrderAllDto>> GetDraftAsync(OrderDraftFilterDto dto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText =
            @"EXEC dbo.SP_OrderDraft @BusinessUnitId, @ItemCode, @OrderTypeId, @UserId, @DateFrom, @DateTo";

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
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderAllDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReader(reader));
        }

        return data;
    }

    public async Task<OrderIUDResponse> SaveOrderMainAsync(OrderMainDto orderMainDto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"DECLARE @NewBidMainId INT,@NewBidNo NVARCHAR(15)
    
                                SP_OrderMain_IUD @OrderMainId INT = NULL,
                                @BusinessUnitId INT = NULL,
                                @OrderTypeId INT = NULL,
                                @OrderDate DATE = NULL,
                                @Emergency INT = NULL,
                                @Buyer NVARCHAR(15) = NULL,
                                @ApproveStageMainId INT = NULL,
                                @Comment NVARCHAR(2000) = NULL,
                                @VendorCode NVARCHAR(15) = NULL,
                                @Currency NVARCHAR(5) = NULL,
                                @DiscountType INT = NULL,
                                @DiscountValue DECIMAL(18, 3) = NULL,
                                @DeliveryTermId INT = NULL,
                                @DeliveryTime NVARCHAR(500) = NULL,
                                @PaymentTermId INT = NULL,
                                @ExpectedCost DECIMAL(18, 3) = NULL,
                                @DeliveryDate DATE = NULL,
                                @DesiredDeliverydate DATE = NULL,
                                @BidMainId INT = NULL,
                                @RFQMainId INT = NULL,
                                @UserId INT,
                                @NewOrderMainId INT OUTPUT,
                                @NewOrderNo NVARCHAR(15) OUTPUT
    
                                SELECT	@NewBidMainId as N'@NewBidMainId',@NewBidNo as N'@NewBidNo'";

        command.Parameters.AddWithValue(command, "@OrderMainId", orderMainDto.OrderMainId);
        command.Parameters.AddWithValue(command, "@BusinessUnitId", orderMainDto.BusinessUnitId);
        command.Parameters.AddWithValue(command, "@OrderTypeId", orderMainDto.OrderTypeId);
        command.Parameters.AddWithValue(command, "@OrderDate", orderMainDto.OrderDate);
        command.Parameters.AddWithValue(command, "@Emergency", orderMainDto.Emergency);
        command.Parameters.AddWithValue(command, "@Buyer", orderMainDto.Buyer);
        command.Parameters.AddWithValue(command, "@ApproveStageMainId", orderMainDto.ApproveStageMainId);
        command.Parameters.AddWithValue(command, "@Comment", orderMainDto.Comment);
        command.Parameters.AddWithValue(command, "@VendorCode", orderMainDto.VendorCode);
        command.Parameters.AddWithValue(command, "@Currency", orderMainDto.Currency);
        command.Parameters.AddWithValue(command, "@DiscountType", orderMainDto.DiscountType);
        command.Parameters.AddWithValue(command, "@DiscountValue", orderMainDto.DiscountValue);
        command.Parameters.AddWithValue(command, "@DeliveryTermId", orderMainDto.DeliveryTermId);
        command.Parameters.AddWithValue(command, "@DeliveryTime", orderMainDto.DeliveryTime);
        command.Parameters.AddWithValue(command, "@PaymentTermId", orderMainDto.PaymentTermId);
        command.Parameters.AddWithValue(command, "@ExpectedCost", orderMainDto.ExpectedCost);
        command.Parameters.AddWithValue(command, "@DeliveryDate", orderMainDto.DeliveryDate);
        command.Parameters.AddWithValue(command, "@DesiredDeliverydate", orderMainDto.DesiredDeliveryDate);
        command.Parameters.AddWithValue(command, "@BidMainId", orderMainDto.BidMainId);
        command.Parameters.AddWithValue(command, "@RFQMainId", orderMainDto.RFQMainId);
        command.Parameters.AddWithValue(command, "@UserId", userId);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
            return GetOrderSaveResponse(reader);

        return null;
    }

    public async Task<bool> SaveOrderDetailsAsync(List<OrderDetailDto> orderDetails)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"SET NOCOUNT OFF EXEC dbo.SP_OrderDetails_IUD @OrderMainId, @Data";
        command.Parameters.AddWithValue(command, "@OrderMainId", orderDetails[0].OrderMainId);
        command.Parameters.AddTableValue(command, "@Data", "dbo.OrderDetailsType2", orderDetails.ConvertToDataTable());

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> ChangeOrderMainStatusAsync(ChangeOrderMainStatusDto statusDto, int userId, int orderMainId,
        int sequence)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"SET NOCOUNT OFF dbo.SP_OrderMainApprove 
            @OrderMainId,
            @UserId,
            @ApproveStatusId,
            @Comment,
            @Sequence,
            @RejectReasonId";

        command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);
        command.Parameters.AddWithValue(command, "@UserId", userId);
        command.Parameters.AddWithValue(command, "@ApproveStatusId", statusDto.ApproveStatusId);
        command.Parameters.AddWithValue(command, "@Comment", statusDto.Comment);
        command.Parameters.AddWithValue(command, "@Sequence", sequence);
        command.Parameters.AddWithValue(command, "@RejectReasonId", statusDto.RejectReasonId);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> SendToApproveAsync(List<int> orderMainIdList, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"SET NOCOUNT OFF dbo.SP_OrderSendToApprove
                                @UserId, @OrderMainId";

        var orderMainIdListAsString = string.Join(",", orderMainIdList.Select(x => x.ToString()).ToList());

        command.Parameters.AddWithValue(command, "@UserId", userId);
        command.Parameters.AddWithValue(command, "@OrderMainId", orderMainIdListAsString);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    private static OrderIUDResponse GetOrderSaveResponse(IDataReader reader)
    {
        return new OrderIUDResponse
        {
            OrderMainId = reader.Get<int>("@NewOrderMainId"),
            OrderNo = reader.Get<string>("@NewOrderNo"),
        };
    }

    private static OrderAllDto MapFromReader(IDataReader reader)
    {
        return new OrderAllDto
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
        };
    }
}
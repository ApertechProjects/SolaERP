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
            data.Add(MapFromReaderForOrderAllDto(reader));
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
            data.Add(MapFromReaderForOrderAllDto(reader));
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
            data.Add(MapFromReaderForOrderAllDto(reader));
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
            data.Add(MapFromReaderForOrderAllDto(reader));
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
            data.Add(MapFromReaderForOrderAllDto(reader));
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
            data.Add(MapFromReaderForOrderAllDto(reader));
        }

        return data;
    }

    public async Task<OrderIUDResponse> SaveOrderMainAsync(OrderMainDto orderMainDto, int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"DECLARE @NewOrderMainId INT, @NewOrderNo NVARCHAR(15)
    
                                EXEC SP_OrderMain_IUD @OrderMainId,
                                @BusinessUnitId,
                                @OrderTypeId,
                                @OrderDate,
                                @Emergency,
                                @Buyer,
                                @ApproveStageMainId,
                                @Comment,
                                @VendorCode,
                                @Currency,
                                @DiscountType,
                                @DiscountValue,
                                @DeliveryTermId,
                                @DeliveryTime,
                                @PaymentTermId,
                                @ExpectedCost,
                                @DeliveryDate,
                                @DesiredDeliverydate,
                                @BidMainId,
                                @RFQMainId,
                                @UserId,
                                @OrderPrint,
                                @NewOrderMainId = @NewOrderMainId OUTPUT,
		                        @NewOrderNo = @NewOrderNo OUTPUT
                                        
                                SELECT	@NewOrderMainId as N'@NewOrderMainId',@NewOrderNo as N'@NewOrderNo'";

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
        command.Parameters.AddWithValue(command, "@OrderPrint", orderMainDto.OrderPrint);

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
        var a = orderDetails.ConvertToDataTable();
        command.Parameters.AddTableValue(command, "@Data", "OrderDetailsType2", a);

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

    public async Task<List<OrderHeadLoaderDto>> GetHeaderLoadAsync(int orderMainId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderHeaderLoad @OrderMainId";


        command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderHeadLoaderDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReaderForOrderHeaderLoaderDto(reader));
        }

        return data;
    }

    public async Task<List<OrderCreateRequestListDto>> GetOrderCreateListForRequestAsync(OrderCreateListRequest dto)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderCreateList  
                                @BusinessUnitId,
                                @DateFrom,
                                @DateTo,
                                @Source";


        command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);
        command.Parameters.AddWithValue(command, "@Source", 0);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderCreateRequestListDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReaderForOrderCreateRequestListDto(reader));
        }

        return data;
    }


    public async Task<List<OrderCreateBidListDto>> GetOrderCreateListForBidsAsync(OrderCreateListRequest dto)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC dbo.SP_OrderCreateList  
                                @BusinessUnitId,
                                @DateFrom,
                                @DateTo,
                                @Source";


        command.Parameters.AddWithValue(command, "@BusinessUnitId", dto.BusinessUnitId);
        command.Parameters.AddWithValue(command, "@DateFrom", dto.DateFrom);
        command.Parameters.AddWithValue(command, "@DateTo", dto.DateTo);
        command.Parameters.AddWithValue(command, "@Source", 1);

        await using DbDataReader reader = await command.ExecuteReaderAsync();
        List<OrderCreateBidListDto> data = new();
        while (await reader.ReadAsync())
        {
            data.Add(MapFromReaderForOrderCreateBidsListDto(reader));
        }

        return data;
    }

    private static OrderCreateRequestListDto MapFromReaderForOrderCreateRequestListDto(IDataReader reader)
    {
        return new OrderCreateRequestListDto
        {
            ItemCode = reader.Get<string>("ItemCode"),
            Buyer = reader.Get<string>("Buyer"),
            Amount = reader.Get<decimal>("Amount"),
            Condition = reader.Get<int>("Condition"),
            Description = reader.Get<string>("Description"),
            Location = reader.Get<string>("Location"),
            Priority = reader.Get<int>("Priority"),
            AccountCode = reader.Get<string>("AccountCode"),
            AlternativeItem = reader.Get<bool>("AlternativeItem"),
            AvailableQuantity = reader.Get<decimal>("AvailableQuantity"),
            ItemName1 = reader.Get<string>("ItemName"),
            ItemName2 = reader.Get<string>("ItemName2"),
            RequestLineNo = reader.Get<string>("RequestLineNo"),
            OrderQuantity = reader.Get<decimal>("OrderQuantity"),
            OriginalQuantity = reader.Get<decimal>("OriginalQuantity"),
            RemainingBudget = reader.Get<decimal>("RemainingBudget"),
            RemainingQuantity = reader.Get<decimal>("RemainingQuantity"),
            RequestDate = reader.Get<DateTime>("RequestDate"),
            RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
            RequestedDate = reader.Get<DateTime>("RequestedDate"),
            RequestNo = reader.Get<string>("RequestNo"),
            RequestQuantity = reader.Get<decimal>("RequestQuantity"),
            TotalBudget = reader.Get<decimal>("TotalBudget"),
            BusinessCategoryId = reader.Get<int>("BusinessCategoryId"),
            ManualUP = reader.Get<decimal>("ManualUP"),
            QuantityFromStock = reader.Get<decimal>("QuantityFromStock"),
            RequestDetailId = reader.Get<int>("RequestDetailId"),
            RequestMainId = reader.Get<int>("RequestMainId"),
            DefaultUOM = reader.Get<string>("DefaultUOM"),
            RFQQuantity = reader.Get<decimal>("RFQQuantity"),
            Ordertype = reader.Get<string>("Ordertype"),
            RequestComment = reader.Get<string>("RequestComment"),
            RequestType = reader.Get<string>("RequestType"),
            RequestTypeId = reader.Get<int>("RequestTypeId"),
            TypeKey = reader.Get<string>("TypeKey"),
            RUOM = reader.Get<string>("RUOM"),
            AnalysisCode1Id = reader.Get<int>("AnalysisCode1Id"),
            AnalysisCode2Id = reader.Get<int>("AnalysisCode2Id"),
            AnalysisCode3Id = reader.Get<int>("AnalysisCode3Id"),
            AnalysisCode4Id = reader.Get<int>("AnalysisCode4Id"),
            AnalysisCode5Id = reader.Get<int>("AnalysisCode5Id"),
            AnalysisCode6Id = reader.Get<int>("AnalysisCode6Id"),
            AnalysisCode7Id = reader.Get<int>("AnalysisCode7Id"),
            AnalysisCode8Id = reader.Get<int>("AnalysisCode8Id"),
            AnalysisCode9Id = reader.Get<int>("AnalysisCode9Id"),
            AnalysisCode10Id = reader.Get<int>("AnalysisCode10Id")
        };
    }

    private static OrderCreateBidListDto MapFromReaderForOrderCreateBidsListDto(DbDataReader reader)
    {
        return new OrderCreateBidListDto
        {
            ItemCode = reader.Get<string>("ItemCode"),
            Condition = reader.Get<int>("Condition"),
            AlternativeItem = reader.Get<bool>("AlternativeItem"),
            ItemName = reader.Get<string>("ItemName"),
            ItemName2 = reader.Get<string>("Itemname2"),
            LineNo = reader.Get<int>("LineNo"),
            RequestNo = reader.Get<string>("RequestNo"),
            RequestDetailId = reader.Get<int>("RequestDetailId"),
            RequestMainId = reader.Get<int>("RequestMainId"),
            DefaultUOM = reader.Get<string>("DefaultUOM"),
            DiscountType = reader.Get<int>("DiscountType"),
            ComparisonNo = reader.Get<string>("ComparisonNo"),
            Status = reader.Get<int>("Status"),
            DiscountValue = reader.Get<int>("DiscountValue"),
            ApproveStatus = reader.Get<int>("ApproveStatus"),
            BidNo = reader.Get<string>("BidNo"),
            Quantity = reader.Get<decimal>("Quantity"),
            ConversionRate = reader.Get<decimal>("ConversionRate"),
            DiscountedAmount = reader.Get<decimal>("DiscountedAmount"),
            LineDescription = reader.Get<string>("LineDescription"),
            BidMainId = reader.Get<int>("BidMainId"),
            RequestLine = reader.Get<int>("RequestLine"),
            TotalAmount = reader.Get<decimal>("TotalAmount"),
            UnitPrice = reader.Get<decimal>("UnitPrice"),
            AlternativeItemCode = reader.Get<string>("AlternativeItemCode"),
            AlternativeItemName = reader.Get<string>("AlternativeItemName"),
            BidComparisonId = reader.Get<int>("BidComparisonId"),
            BidDetailId = reader.Get<int>("BidDetailId"),
            ConvertedUnitPrice = reader.Get<decimal>("ConvertedUnitPrice"),
            SingleUnitPrice = reader.Get<decimal>("SingleUnitPrice"),
            ConvertedQTY = reader.Get<decimal>("ConvertedQTY"),
            PUOM = reader.Get<string>("PUOM"),
            RequestUOM = reader.Get<string>("RequestUOM"),
            RFQDetailId = reader.Get<int>("RFQDetailId")
        };
    }

    private static OrderHeadLoaderDto MapFromReaderForOrderHeaderLoaderDto(IDataReader reader)
    {
        return new OrderHeadLoaderDto
        {
            OrderMainId = reader.Get<int>("OrderMainId"),
            OrderTypeId = reader.Get<int>("OrderTypeId"),
            OrderNo = reader.Get<string>("OrderNo"),
            OrderDate = reader.Get<DateTime>("OrderDate"),
            Emergency = reader.Get<int>("Emergency"),
            EntryDate = reader.Get<DateTime>("EntryDate"),
            EnteredBy = reader.Get<string>("EnteredBy"),
            ReasonCode = reader.Get<string>("ReasonCode"),
            ReasonName = reader.Get<string>("ReasonName"),
            Buyer = reader.Get<string>("Buyer"),
            Comment = reader.Get<string>("Comment"),
            ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
            VendorCode = reader.Get<string>("VendorCode"),
            VendorName = reader.Get<string>("VendorName"),
            CompanyAddress = reader.Get<string>("CompanyAddress"),
            ContactPerson = reader.Get<string>("ContactPerson"),
            Email = reader.Get<string>("Email"),
            Country = reader.Get<string>("Country"),
            PhoneNo = reader.Get<string>("PhoneNo"),
            TaxId = reader.Get<string>("TaxId"),
            TaxCode = reader.Get<string>("TaxCode"),
            Currency = reader.Get<string>("Currency"),
            DiscountType = reader.Get<int>("DiscountType"),
            DiscountValue = reader.Get<decimal>("DiscountValue"),
            DeliveryTermId = reader.Get<int>("DeliveryTermId"),
            DeliveryTime = reader.Get<string>("DeliveryTime"),
            PaymentTermId = reader.Get<int>("PaymentTermId"),
            ExpectedCost = reader.Get<decimal>("ExpectedCost"),
            DeliveryDate = reader.Get<DateTime>("DeliveryDate"),
            DesiredDeliveryDate = reader.Get<DateTime>("DesiredDeliveryDate"),
            RFQMainId = reader.Get<int>("RFQMainId"),
            RFQNo = reader.Get<string>("RFQNo"),
            RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
            RFQDate = reader.Get<DateTime>("RFQDate"),
            RequiredOnSiteDate = reader.Get<DateTime>("RequiredOnSiteDate"),
            ComparisonNo = reader.Get<int>("ComparisonNo"),
            Comparisondeadline = reader.Get<DateTime>("Comparisondeadline"),
            ComparisonDate = reader.Get<DateTime>("ComparisonDate"),
            BidMainId = reader.Get<int>("BidMainId"),
            Status = reader.Get<int>("Status"),
            ApproveStatus = reader.Get<int>("ApproveStatus"),
            BusinessUnitId = reader.Get<int>("BusinessUnitId"),
            InvoiceStatus = reader.Get<int>("BusinessUnitId"),
            GRNStatus = reader.Get<int>("GRNStatus"),
            OrderPrint = reader.Get<bool>("OrderPrint")
        };
    }

    private static OrderIUDResponse GetOrderSaveResponse(IDataReader reader)
    {
        return new OrderIUDResponse
        {
            OrderMainId = reader.Get<int>("@NewOrderMainId"),
            OrderNo = reader.Get<string>("@NewOrderNo"),
        };
    }

    private static OrderAllDto MapFromReaderForOrderAllDto(IDataReader reader)
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
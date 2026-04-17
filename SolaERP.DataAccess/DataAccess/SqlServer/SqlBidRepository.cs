using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Globalization;
using SolaERP.Application.Dtos.RFQ;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBidRepository : IBidRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBidRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BidIUDResponse> BidMainIUDAsync(BidMain entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"DECLARE @NewBidMainId INT,@NewBidNo NVARCHAR(15)

                                        EXEC SP_BidMain_IUD @BidMainId,
                                        @BusinessUnitId,
                                        @RFQMainId,
                                        @BidNo,
                                        @OperatorComment,
                                        @VendorCode,
                                        @CurrencyCode,
                                        @DiscountType,
                                        @DiscountValues,
                                        @DeliveryTerms,
                                        @DeliveryTime,
                                        @PaymentTerms,
                                        @ExpectedCost,
                                        @Status,
                                        @ApprovalStatus,
                                        @ApproveStageMainId,
                                        @UserId,
                                        @NewBidMainId = @NewBidMainId OUTPUT,
		                                @NewBidNo = @NewBidNo OUTPUT
                                        
SELECT	@NewBidMainId as N'@NewBidMainId',@NewBidNo as N'@NewBidNo'";

            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", entity.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@RFQMainId", entity.RFQMainId);
            command.Parameters.AddWithValue(command, "@BidNo", entity.BidNo);
            command.Parameters.AddWithValue(command, "@OperatorComment", entity.OperatorComment);
            command.Parameters.AddWithValue(command, "@VendorCode", entity.VendorCode);
            command.Parameters.AddWithValue(command, "@CurrencyCode", entity.CurrencyCode);
            command.Parameters.AddWithValue(command, "@DiscountType", entity.DiscountType);
            command.Parameters.AddWithValue(command, "@DiscountValues", entity.DiscountValues);
            command.Parameters.AddWithValue(command, "@DeliveryTerms", entity.DeliveryTerms);
            command.Parameters.AddWithValue(command, "@DeliveryTime", entity.DeliveryTime);
            command.Parameters.AddWithValue(command, "@PaymentTerms", entity.PaymentTerms);
            command.Parameters.AddWithValue(command, "@ExpectedCost", entity.ExpectedCost);
            command.Parameters.AddWithValue(command, "@Status", entity.Status);
            command.Parameters.AddWithValue(command, "@ApprovalStatus", entity.ApprovalStatus);
            command.Parameters.AddWithValue(command, "@ApproveStageMainId", entity.ApproveStageMainId);
            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return GetBidSaveResponse(reader);
            return null;
        }

        public async Task<bool> BidDisqualifyAsync(BidDisqualify entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;

            command.CommandText = @"SET NOCOUNT OFF EXEC SP_BidDisqualify @BidMainId,
                                    @Discualified,
                                    @DiscualificationReason,
                                    @UserId";

            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@Discualified", entity.Discualified);
            command.Parameters.AddWithValue(command, "@DiscualificationReason", entity.DiscualificationReason);
            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> SaveBidDetailsAsync(List<BidDetail> details)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;

            command.CommandText = "SET NOCOUNT OFF EXEC SP_BidDetails_IUD @Data";
            command.Parameters.AddTableValue(command, "@Data", "BidDetailsType", details.ConvertToDataTable());

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<BidAll>> GetAllAsync(BidAllFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidAll @BusinessUnitId,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @Status,
                                        @ApproveStatus";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
            command.Parameters.AddWithValue(command, "@Status", filter.Status);
            command.Parameters.AddWithValue(command, "@ApproveStatus", filter.ApproveStatus);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidAll> data = new();
            while (reader.Read())
                data.Add(GetBidFromReader(reader));
            return data;
        }

        public async Task<List<BidAll>> GetDraftAsync(BidAllFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidDraft @BusinessUnitId,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @ApproveStatus";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
            command.Parameters.AddWithValue(command, "@ApproveStatus", filter.ApproveStatus);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidAll> data = new();
            while (reader.Read())
                data.Add(GetBidFromReader(reader));
            return data;
        }

        public async Task<List<BidAll>> GetSubmittedAsync(BidAllFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidSubmitted @BusinessUnitId,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @ApproveStatus";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
            command.Parameters.AddWithValue(command, "@ApproveStatus", filter.ApproveStatus);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidAll> data = new();
            while (reader.Read())
                data.Add(GetBidFromReader(reader));
            return data;
        }

        public async Task<List<BidDetailsLoad>> GetBidDetailsAsync(BidDetailsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidDetailsLoad @BidMainId";

            command.Parameters.AddWithValue(command, "@BidMainId", filter.BidMainId);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidDetailsLoad> data = new();
            while (reader.Read())
                data.Add(new BidDetailsLoad
                {
                    AlternativeItem = reader.Get<bool>("AlternativeItem"),
                    AlternativeItemCode = reader.Get<string>("AlternativeItemCode"),
                    AlternativeItemName = reader.Get<string>("AlternativeItemName"),
                    BidDetailId = reader.Get<int>("BidDetailId"),
                    BidMainId = reader.Get<int>("BidMainId"),
                    Condition = reader.Get<int>("Condition"),
                    ConversionRate = reader.Get<decimal>("ConversionRate"),
                    ConvertedQTY = reader.Get<decimal>("ConvertedQTY"),
                    ConvertedUnitPrice = reader.Get<decimal>("ConvertedUnitPrice"),
                    Description = reader.Get<string>("Description"),
                    DiscountedAmount = reader.Get<decimal>("DiscountedAmount"),
                    DiscountType = reader.Get<int>("DiscountType"),
                    DiscountValue = reader.Get<decimal>("DiscountValue"),
                    ItemCode = reader.Get<string>("ItemCode"),
                    LineDescription = reader.Get<string>("LineDescription"),
                    LineNo = reader.Get<int>("LineNo"),
                    PUOM = reader.Get<string>("PUOM"),
                    Quantity = reader.Get<decimal>("Quantity"),
                    RFQDetailId = reader.Get<int>("RFQDetailId"),
                    RFQLine = reader.Get<int>("RFQLine"),
                    RFQQuantity = reader.Get<decimal>("RFQQuantity"),
                    SingleUnitPrice = reader.Get<decimal>("SingleUnitPrice"),
                    TotalAmount = reader.Get<decimal>("TotalAmount"),
                    UnitPrice = reader.Get<decimal>("UnitPrice"),
                    UOM = reader.Get<string>("UOM")
                });
            return data;
        }

        public async Task<BidMainLoad> GetMainLoadAsync(int bidMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidMain_Load @BidMainId";

            command.Parameters.AddWithValue(command, "@BidMainId", bidMainId);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidMainLoad> data = new();
            if (reader.Read())
                return GetBidMainLoadFromReader(reader);
            return new BidMainLoad();
        }

        private BidAll GetBidFromReader(DbDataReader reader)
        {
            return new BidAll
            {
                BidMainId = reader.Get<int>("BidMainId"),
                Buyer = reader.Get<string>("Buyer"),
                LineNo = reader.Get<long>("LineNo"),
                RFQNo = reader.Get<string>("RFQNo"),
                BidNo = reader.Get<string>("BidNo"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                DeliveryTerms = reader.Get<string>("DeliveryTerms"),
                DeliveryTime = reader.Get<string>("DeliveryTime"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                ExpectedCost = reader.Get<decimal>("ExpectedCost"),
                Status = reader.Get<string>("Status"),
                ApproveStatusName = reader.Get<string>("ApproveStatus"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                Emergency = reader.Get<int>("Emergency"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                OrderNo = reader.Get<string>("OrderNo"),
                RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                Discount = reader.Get<decimal>("Discount"),
                DiscountedPrice = reader.Get<decimal>("DiscountedPrice"),
                DiscountedPriceBase = reader.Get<decimal>("DiscountedPriceBase"),
                TotalPrice = reader.Get<decimal>("TotalPrice"),
                TotalPriceBase = reader.Get<decimal>("TotalPriceBase"),
                HasAttachments = reader.Get<bool>("HasAttachments")
            };
        }

        private BidMainLoad GetBidMainLoadFromReader(DbDataReader reader)
        {
            return new BidMainLoad
            {
                BidMainId = reader.Get<int>("BidMainId"),
                BidNo = reader.Get<string>("BidNo"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                VendorCode = reader.Get<string>("VendorCode"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                DeliveryTerms = reader.Get<string>("DeliveryTerms"),
                DeliveryTime = reader.Get<string>("DeliveryTime"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                ExpectedCost = reader.Get<decimal>("ExpectedCost"),
                Status = reader.Get<int>("Status"),
                ApprovalStatus = reader.Get<int>("ApprovalStatus"),
                ApproveStageMainId = reader.Get<int>("ApproveStageMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                DiscountType = reader.Get<int>("DiscountType"),
                DiscountValues = reader.Get<decimal>("DiscountValues"),
                RFQMainId = reader.Get<int>("RFQMainId"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                OrderNo = reader.Get<string>("OrderNo"),
                DiscualificationReason = reader.Get<string>("DiscualificationReason"),
                Discualified = reader.Get<bool>("Discualified"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RfqComment = reader.Get<string>("RfqComment")
            };
        }

        private BidIUDResponse GetBidSaveResponse(IDataReader reader)
        {
            return new()
            {
                Id = reader.Get<int>("@NewBidMainId"),
                BidNo = reader.Get<string>("@NewBidNo"),
            };
        }

        public async Task<List<BidRFQListLoad>> GetRFQListForBidAsync(BidRFQListFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidRFQListLoad @UserId,@businessUnitId";

            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);
            command.Parameters.AddWithValue(command, "@businessUnitId", filter.BusinessUnitId);

            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidRFQListLoad> data = new();
            while (reader.Read())
                data.Add(new BidRFQListLoad
                {
                    RFQMainId = reader.Get<int>("RFQMainId"),
                    BidCount = reader.Get<int>("BidCount"),
                    BusinessCategoryName = reader.Get<string>("BusinessCategoryName"),
                    Buyer = reader.Get<string>("Buyer"),
                    Emergency = reader.Get<string>("Emergency"),
                    RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                    RFQNo = reader.Get<string>("RFQNo")
                });
            return data;
        }

        public async Task OrderCreateFromApproveBidAsync(int bidMainId, int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;

            command.CommandText = @"EXEC dbo.SP_OrderCreateFromApprovedBid @BidMainId, @Userid";

            command.Parameters.AddWithValue(command, "@BidMainId", bidMainId);
            command.Parameters.AddWithValue(command, "@Userid", userId);

            await _unitOfWork.SaveChangesAsync();

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<int>> GetDetailIds(int id)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"select BidDetailId from Procurement.BidDetails where BidMainId = @BidDetailId";
            command.Parameters.AddWithValue(command, "@BidDetailId", id);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            List<int> datas = new();
            while (await reader.ReadAsync())
            {
                datas.Add(reader.Get<int>("BidDetailId"));
            }

            return datas;
        }

        public async Task<List<BidMainDto>> GetBidByRFQMainIdAndVendorCode(int rfqMainId, string vendorCode)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"select BidDetailId from Procurement.BidMain where RFQMainId = @RFQMainId and VendorCode = @VendorCode";
            command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            List<BidMainDto> datas = new();
            while (reader.Read())
                datas.Add(new BidMainDto
                {
                    RFQMainId = reader.Get<int>("RFQMainId"),
                    VendorCode = reader.Get<String>("VendorCode"),
                });

            return datas;
        }

        public async Task<bool> GetBidCheckExistsByRFQMainIdAndVendorCode(int rfqMainId, string vendorCode)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"select count(*) ac count from Procurement.BidMain where RFQMainId = @RFQMainId and VendorCode = @VendorCode";
            command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            // await using DbDataReader reader = await command.ExecuteReaderAsync();
            // List<BidMainDto> datas = new();
            // while (reader.Read())
            //     datas.Add(new BidMainDto
            //     {
            //         RFQMainId = reader.Get<int>("RFQMainId"),
            //         VendorCode = reader.Get<String>("VendorCode"),
            //     });

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<RFQVendorEmailDto>> GetBidsByRFQMainIdAsync(List<int> rfqMainIds)
        {
            var idListForSql = string.Join(",", rfqMainIds);

            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = $@"
                                    select 
                                        b.RFQMainId,
                                        b.VendorCode,
                                        v.VendorName,
                                        rfqm.RFQDeadline,
                                        au.Email,
                                        b.BidNo,
                                        rfqm.RFQNo
                                    from Procurement.BidMain as b
                                             inner join Procurement.RFQMain rfqm on rfqm.RFQMainId = b.RFQMainId
                                             inner join Procurement.Vendors as v on b.VendorCode = v.VendorCode
                                             inner join Config.AppUser as au on au.VendorId = v.VendorId
                                    where b.RFQMainId in ({idListForSql})
                                    and v.Status = 2
                                    ";


            await using DbDataReader reader = await command.ExecuteReaderAsync();
            List<RFQVendorEmailDto> datas = new();
            while (reader.Read())
                datas.Add(new RFQVendorEmailDto
                {
                    RFQMainId = reader.Get<int>("RFQMainId"),
                    VendorCode = reader.Get<string>("VendorCode"),
                    VendorName = reader.Get<string>("VendorName"),
                    RfqDeadline = reader.Get<DateTime>("RfqDeadline"),
                    VendorEmail = reader.Get<string>("Email"),
                    BidNo = reader.Get<string>("BidNo"),
                    RFQNo = reader.Get<string>("RFQNo"),
                });

            return datas;
        }

        public async Task<List<BidComparisonBidHeaderDto>> GetBidHeaderByRfqMainIdAsync(int rfqMainId)
        {
            var data = new List<BidComparisonBidHeaderDto>();

            await using var headerCommand = _unitOfWork.CreateCommand() as DbCommand;
            headerCommand.CommandText = @"
                            SELECT
                                BM.BidMainId,
                                BM.DeliveryTime,
                                DT.DeliverytermName,
                                PT.PaymentTermName,
                                BM.OperatorComment,
                                SUMS.total,
                                SUMS.discount,
                                SUMS.discountedAmount,
                                TAX.WithHoldingTax AS VendorWHTRate,
                                TAX.Tax AS taxRate,
                                BM.ExpectedCost,
                                BM.CurrencyCode AS currencyKey,
                                V.VendorName
                            FROM Procurement.BidMain BM

                                     LEFT JOIN Register.DeliveryTerms DT
                                               ON DT.DeliveryTermCode = BM.DeliveryTerms

                                     LEFT JOIN Register.PaymentTerms PT
                                               ON PT.PaymentTermCode = BM.PaymentTerms

                                     left join Procurement.Vendors V on BM.VendorCode = V.VendorCode
                                     LEFT JOIN (
                                SELECT
                                    BD.BidMainId,
                                    SUM(ISNULL(BD.totalAmount, 0)) AS total,
                                    SUM(ISNULL(BD.totalAmount, 0) - ISNULL(BD.discountedAmount, 0)) AS discount,
                                    SUM(ISNULL(BD.discountedAmount, 0)) AS discountedAmount
                                FROM Procurement.BidDetails BD
                                GROUP BY BD.BidMainId
                            ) SUMS
                                               ON SUMS.BidMainId = BM.BidMainId
                                     Left Join (Select BM.BidMainId,
                                                       WHT.WithHoldingTax,
                                                       T.Tax
                                                from Procurement.BidMain BM
                                                         left join Procurement.Vendors V on BM.VendorCode = V.VendorCode
                                                         Left join Register.Taxes T on T.TaxId = V.TaxId
                                                         Left join Register.WithHoldingTax WHT on V.WithHoldingTaxId = WHT.WithHoldingTaxId
                                                         Left Join Config.BusinessUnits BU on BU.BusinessUnitId = BM.BusinessUnitId
                                                where BM.RFQMainId = @RFQMainId and BU.VATAccount is not null)
                                TAX
                                               ON TAX.BidMainId = BM.BidMainId
                            where BM.RFQMainId = @RFQMainId";
            headerCommand.Parameters.AddWithValue(headerCommand, "@RFQMainId", rfqMainId);

            await using (DbDataReader reader = await headerCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    data.Add(new BidComparisonBidHeaderDto
                    {
                        bidMainId = reader.Get<int>("BidMainId"),
                        deliveryTime = reader.Get<string>("DeliveryTime"),
                        deliveryTermName = reader.Get<string>("DeliverytermName"),
                        paymentTermName = reader.Get<string>("PaymentTermName"),
                        operatorComment = reader.Get<string>("OperatorComment"),
                        total = reader.Get<decimal>("total"),
                        discount = reader.Get<decimal>("discount"),
                        discountedAmount = reader.Get<decimal>("discountedAmount"),
                        vendorWHTRate = reader.Get<decimal>("vendorWHTRate"),
                        taxRate = reader.Get<decimal>("taxRate"),
                        expectedCost = reader.Get<decimal>("expectedCost"),
                        whtRate = reader.Get<decimal>("vendorWHTRate"),
                        currencyKey = reader.Get<string>("currencyKey"),
                        vendorName = reader.Get<string>("vendorName"),
                        BidDetails = new List<BidComparisonBidDetailsDto>()
                    });
                }
            }
            
            
            
            
            await using var detailCommand = _unitOfWork.CreateCommand() as DbCommand;
            detailCommand.CommandText = @"
                                   Select BD.BidMainId,
                                   BD.AlternativeItemName,
                                   BD.DiscountValue,
                                   BD.ItemCode,
                                   BD.LineDescription AS AlternativeDescription,
                                   BD.UnitPrice,
                                   BD.TotalAmount,
                                   BD.Quantity,
                                   BD.PUOM,
                                   BD.ApproveStatus,
                                   BD.RFQDetailId,
                                   BCB.IsSelected AS Selected
                            from Procurement.BidDetails BD
                                     inner join Procurement.BidMain BM on BD.BidMainId = BM.BidMainId
                                     Left Join Procurement.BidComparison BC on BC.RFQMainId = BM.RFQMainId
                                     Left Join Procurement.BidComparisonBids BCB on BC.BidComparisonId = BCB.BidComparisonId
                                   where BM.RFQMainId = @RFQMainId";
            detailCommand.Parameters.AddWithValue(detailCommand, "@RFQMainId", rfqMainId);

            var detailsByBidMainId = new Dictionary<int, List<BidComparisonBidDetailsDto>>();
            await using (DbDataReader reader = await detailCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var bidMainId = reader.Get<int>("BidMainId");
                    if (!detailsByBidMainId.TryGetValue(bidMainId, out var list))
                    {
                        list = new List<BidComparisonBidDetailsDto>();
                        detailsByBidMainId[bidMainId] = list;
                    }

                    list.Add(new BidComparisonBidDetailsDto
                    {
                        BidMainId = bidMainId,
                        AlternativeDescription = reader.Get<string>("AlternativeItemName"),
                        DiscountValue = reader.Get<decimal>("DiscountValue"),
                        ItemId = reader.Get<string>("ItemCode"),
                        UnitPrice = reader.Get<decimal>("UnitPrice"),
                        TotalAmount = reader.Get<decimal>("TotalAmount"),
                        Quantity = reader.Get<decimal>("Quantity"),
                        PUOMName = reader.Get<string>("PUOM"),
                        ApproveStatusId = reader.Get<int>("ApproveStatus"),
                        RFQDetailId = reader.Get<int>("RFQDetailId"),
                        Selected = reader.Get<bool?>("Selected") ?? false
                    });
                }
            }

            var rateByCurrency = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            var localDateNow = DateTime.Now.Date;

            foreach (var header in data)
            {
                header.BidDetails = detailsByBidMainId.TryGetValue(header.bidMainId, out var list)
                    ? list
                    : new List<BidComparisonBidDetailsDto>();

                header.whtRate = header.vendorWHTRate;

                var totalWithWht = header.discountedAmount +
                                   (header.discountedAmount * header.whtRate / 100m);
                header.totalWithWHT = totalWithWht.ToString(CultureInfo.InvariantCulture);

                header.taxValue = totalWithWht * header.taxRate / 100m;

                var grandTotal = totalWithWht + header.taxValue + header.expectedCost;
                var grandTotalText = grandTotal.ToString(CultureInfo.InvariantCulture);
                header.grantTotal = grandTotalText;

                var currencyCode = header.currencyKey?.Trim();
                var rate = 1m;

                if (!string.IsNullOrWhiteSpace(currencyCode))
                {
                    if (!rateByCurrency.TryGetValue(currencyCode, out rate))
                    {
                        await using var rateCommand = _unitOfWork.CreateCommand() as DbCommand;
                        rateCommand.CommandText = @"
                            Select DT.Rate from Register.DailyRates DT
                            where DT.Date = @Now and Dt.CurrencyCode = @CurrencyCode";
                        rateCommand.Parameters.AddWithValue(rateCommand, "@Now", localDateNow);
                        rateCommand.Parameters.AddWithValue(rateCommand, "@CurrencyCode", currencyCode);

                        var rateResult = await rateCommand.ExecuteScalarAsync();
                        if (rateResult != null &&
                            rateResult != DBNull.Value &&
                            decimal.TryParse(rateResult.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedRate))
                        {
                            rate = parsedRate;
                        }

                        rateByCurrency[currencyCode] = rate;
                    }
                }

                var grandTotalAzn = grandTotal * rate;
                header.grantTotalAZN = grandTotalAzn.ToString(CultureInfo.InvariantCulture);
            }

            return data;
        }

        public async Task<BidRFQDto?> GetVendorCodeForBidAsync(int rfqMainId, int businessUnitId, string vendorCode)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"
                                    SELECT TOP 1
                                    b.RFQMainId,
                                    b.VendorCode,
                                    b.BidNo,
                                    rfqm.RFQNo
                                    FROM Procurement.BidMain AS b
                                    INNER JOIN Procurement.RFQMain rfqm ON rfqm.RFQMainId = b.RFQMainId
                                    WHERE b.RFQMainId = @rfqMainId
                                    AND b.BusinessUnitId = @businessUnitId
                                    AND b.VendorCode = @vendorCode
                                    ";

            var rfqParam = command.CreateParameter();
            rfqParam.ParameterName = "@rfqMainId";
            rfqParam.Value = rfqMainId;
            command.Parameters.Add(rfqParam);

            var buParam = command.CreateParameter();
            buParam.ParameterName = "@businessUnitId";
            buParam.Value = businessUnitId;
            command.Parameters.Add(buParam);

            var vendorParam = command.CreateParameter();
            vendorParam.ParameterName = "@vendorCode";
            vendorParam.Value = vendorCode;
            command.Parameters.Add(vendorParam);
            
            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            var dto = new BidRFQDto
            {
                VendorCode = reader.GetString(reader.GetOrdinal("VendorCode")),
                BidNo = reader.GetString(reader.GetOrdinal("BidNo")),
                RFQNo = reader.GetString(reader.GetOrdinal("RFQNo"))
            };

            return dto;
        }
    }
}

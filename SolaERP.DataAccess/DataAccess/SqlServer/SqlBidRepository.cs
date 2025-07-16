using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
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
                                        b.BidNo
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
    }
}
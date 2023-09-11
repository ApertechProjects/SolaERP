using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return null;
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
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                Emergency = reader.Get<int>("Emergency"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                OrderNo = reader.Get<string>("OrderNo"),
                RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                Discount = reader.Get<decimal>("Discount"),
                DiscountedPrice = reader.Get<decimal>("DiscountedPrice"),
                DiscountedPriceBase = reader.Get<decimal>("DiscountedPriceBase"),
                TotalPrice = reader.Get<decimal>("TotalPrice"),
                TotalPriceBase = reader.Get<decimal>("TotalPriceBase")
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
                EntryDate = reader.Get<DateTime>("EntryDate")
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
            command.CommandText = "EXEC SP_BidRFQListLoad @UserId";

            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

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
                }) ;
            return data;
        }
    }
}

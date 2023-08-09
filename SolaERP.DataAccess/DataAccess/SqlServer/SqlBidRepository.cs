using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
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

        public async Task<BidIUDResponse> AddMainAsync(BidMain entity)
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
                                        @NewBidMainId,
                                        @NewBidNo";

            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@RFQMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@BidNo", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@OperatorComment", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@VendorCode", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@CurrencyCode", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@DiscountType", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@DiscountValues", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@DeliveryTerms", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@DeliveryTime", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@PaymentTerms", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@ExpectedCost", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@Status", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@ApprovalStatus", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@ApproveStageMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@UserId", entity.BidMainId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
                return GetBidSaveResponse(reader);
            return null;
        }

        public async Task<bool> SaveBidDetailsAsync(List<BidDetail> details)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;

            command.CommandText = "SET NOCOUNT OFF EXEC SP_RFQRequestDetails_IUD @Data";
            command.Parameters.AddTableValue(command, "@Data", "BidDetailsType", details.ConvertToDataTable()); ;

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<BidAll>> GetAllAsync(BidAllFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidAll @BusinessUnitId,
                                        @ItemCode,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @Status,
                                        @ApproveStatus";


            command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@ItemCode", filter.ItemCode);
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
                LineNo = reader.Get<long>("LineNo"),
                RFQNo = reader.Get<string>("RFQNo"),
                BidNo = reader.Get<string>("BidNo"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                VendorCode  = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                DeliveryTerms = reader.Get<string>("DeliveryTerms"),
                DeliveryTime = reader.Get<string>("DeliveryTime"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                ExpectedCost = reader.Get<decimal>("ExpectedCost"),
                Status = reader.Get<string>("Status"),
                ApproveStatus = reader.Get<string>("ApproveStatus")
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
                RFQMainId = reader.Get<int>("RFQMainId")
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
    }
}

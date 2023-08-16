using FluentValidation.Validators;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
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
    public class SqlBidComparisonRepository : IBidComparisonRepository
    {

        private readonly IUnitOfWork _unitOfWork;
        public SqlBidComparisonRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> AddComparisonAsync(BidComparisonIUD entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparison_IUD @BidComparisonId,
                                        @RFQMainId,
                                        @ComparisonNo,
                                        @ApproveStageMain,
                                        @ComparisonDate,
                                        @Comparisondeadline,
                                        @SpecialistComment,
                                        @UserId";


            command.Parameters.AddWithValue(command, "@BidComparisonId", entity.BidComparisonId);

            command.Parameters.AddWithValue(command, "@RFQMainId", entity.RFQMainId);

            command.Parameters.AddWithValue(command, "@ComparisonNo", entity.ComparisonNo);

            command.Parameters.AddWithValue(command, "@ApproveStageMain", entity.ApproveStageMain);

            command.Parameters.AddWithValue(command, "@ComparisonDate", entity.ComparisonDate);

            command.Parameters.AddWithValue(command, "@Comparisondeadline", entity.ComparisonDeadline);

            command.Parameters.AddWithValue(command, "@SpecialistComment", entity.SpecialistComment);

            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ApproveComparisonAsync(BidComparisonApprove entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonApprove @BidMainId,
                                        @Sequence,
                                        @ApproveStatus,
                                        @RFQDeatilid,
                                        @UserId";


            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);

            command.Parameters.AddWithValue(command, "@Sequence", entity.Sequence);

            command.Parameters.AddWithValue(command, "@ApproveStatus", entity.ApproveStatus);

            command.Parameters.AddWithValue(command, "@RFQDeatilid", entity.RFQDeatilid);

            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> SendComparisonToApprove(BidComparisonSendToApprove entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonSendToApprove @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", entity.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<BidComparisonBidApprovalsLoad>> GetComparisonBidApprovals(BidComparisonBidApprovalsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidApprovalsLoad>();
            command.CommandText = "EXEC SP_BidComparisonBidApprovalsLoad @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonBidApprovalsLoad
                {
                    ApproveStageName = reader.Get<string>("ApproveStageName"),
                    ApproveStatus = reader.Get<int>("ApproveStatus"),
                    BidMainId = reader.Get<int>("BidMainId"),
                    Sequence = reader.Get<int>("Sequence"),
                    BidDetailId =reader.Get<int>("BidDetailId"),
                    RFQDetailId = reader.Get<int>("RFQDetailId")
                });
            }
            return data;
        }
        
        public async Task<List<BidComparisonBidDetailsLoad>> GetComparisonBidDetails(BidComparisonBidDetailsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidDetailsLoad>();
            command.CommandText = "EXEC SP_BidComparisonBidDetailsLoad @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonBidDetailsLoad
                {
                    BidMainId = reader.Get<int>("BidMainId"),
                    AlternativeDescription = reader.Get<string>("AlternativeDescription"),
                    AlternativeItem = reader.Get<bool>("AlternativeItem"),
                    BidNo = reader.Get<string>("BidNo"),
                    DiscountValue = reader.Get<decimal>("DiscountValue"),
                    Quantity = reader.Get<decimal>("Quantity"),
                    TotalAmount = reader.Get<decimal>("TotalAmount"),
                    UnitPrice = reader.Get<decimal>("UnitPrice"),
                    RFQDetailId = reader.Get<int>("RFQDetailId")
                });;
            }
            return data;
        }

        public async Task<List<BidComparisonRFQDetailsLoad>> GetComparisonRFQDetails(BidComparisonRFQDetailsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonRFQDetailsLoad>();
            command.CommandText = "EXEC SP_BidComparisonRFQDeatilsLoad @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonRFQDetailsLoad
                {
                    LineNo = reader.Get<int>("LineNo"),
                    Budget = reader.Get<decimal>("Budget"),
                    Description = reader.Get<string>("Description"),
                    RemainingBudget = reader.Get<decimal>("RemainingBudget"),
                    UOM = reader.Get<string>("UOM")
                });
            }
            return data;
        }

        public async Task<BidComparisonBidHeaderLoad> GetComparisonBidHeader(BidComparisonBidHeaderFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonBidHeaderLoad @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new BidComparisonBidHeaderLoad
                {
                    BidMainId = reader.Get<int>("BidMainId"),
                    BidNo = reader.Get<string>("BidNo"),
                    BaseAmount = reader.Get<decimal>("BaseAmount"),
                    BaseCurrencyCode = reader.Get<string>("BaseCurrencyCode"),
                    BudgetBalance = reader.Get<decimal>("BudgetBalance"),
                    CurrencyCode = reader.Get<string>("CurrencyCode"),
                    DeliveryTerms = reader.Get<string>("DeliveryTerms"),
                    DeliveryTime = reader.Get<string>("DeliveryTime"),
                    Discount = reader.Get<decimal>("Discount"),
                    DiscountedAmount = reader.Get<decimal>("DiscountedAmount"),
                    DiscualificationReason = reader.Get<string>("DiscualificationReason"),
                    Discualified = reader.Get<bool>("Discualified"),
                    ExpectedCost = reader.Get<decimal>("ExpectedCost"),
                    PaymentTerms = reader.Get<string>("PaymentTerms"),
                    ReportingAmount = reader.Get<decimal>("ReportingAmount"),
                    ReportingCurrencyCode = reader.Get<string>("ReportingCurrencyCode"),
                    Total = reader.Get<decimal>("Total"),
                    VendorName = reader.Get<string>("VendorName")
                };
            }
            return null;
        }

        public async Task<BidComparisonHeaderLoad> GetComparisonHeader(BidComparisonHeaderFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonHeaderLoad @BidComparisonId, @UserId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new BidComparisonHeaderLoad
                {
                    ApproveStageMain = reader.Get<int>("ApproveStageMain"),
                    ApproveStatus = reader.Get<int>("ApproveStatus"),
                    BidComparisonId = reader.Get<int>("BidComparisonId"),
                    BiddingType = reader.Get<int>("BiddingType"),
                    BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                    Buyer = reader.Get<string>("Buyer"),
                    ComparisonDate = reader.Get<DateTime>("ComparisonDate"),
                    ComparisonDeadline = reader.Get<DateTime>("ComparisonDeadline"),
                    ComparisonNo = reader.Get<string>("ComparisonNo"),
                    DesiredDeliveryDate = reader.Get<DateTime>("DesiredDeliveryDate"),
                    Emergency = reader.Get<int>("Emergency"),
                    EnteredBy = reader.Get<string>("EnteredBy"),
                    Entrydate = reader.Get<DateTime>("Entrydate"),
                    ProcurementType = reader.Get<int>("ProcurementType"),
                    RequiredOnSiteDate = reader.Get<DateTime>("RequiredOnSiteDate"),
                    RFQDate = reader.Get<DateTime>("RFQDate"),
                    RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                    RFQMainId = reader.Get<int>("RFQMainId"),
                    RFQNo = reader.Get<string>("RFQNo"),
                    SpecialistComment = reader.Get<string>("SpecialistComment"),
                    Status = reader.Get<int>("Status")
                };
            }
            return null;
        }

    }
}

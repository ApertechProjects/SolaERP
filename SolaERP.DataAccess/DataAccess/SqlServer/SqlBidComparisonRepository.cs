using FluentValidation.Validators;
using MediatR;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBidComparisonRepository : IBidComparisonRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBidComparisonRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<int> AddComparisonAsync(BidComparisonIUD entity)
        {
            using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"SET NOCOUNT OFF  EXEC SP_BidComparison_IUD @BidComparisonId,
                                    @RFQMainId,
                                    @ComparisonNo,
                                    @ApproveStageMain,
                                    @ComparisonDate,
                                    @Comparisondeadline,
                                    @SpecialistComment,
                                    @SingleSourceReasonId,
                                    @UserId,
                                    @NewBidComparisonId = @NewBidComparisonId OUTPUT 
                                    select @NewBidComparisonId as NewBidComparisonId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", entity.BidComparisonId);
            command.Parameters.AddWithValue(command, "@RFQMainId", entity.RFQMainId);
            command.Parameters.AddWithValue(command, "@ComparisonNo", entity.ComparisonNo);
            command.Parameters.AddWithValue(command, "@ApproveStageMain", entity.ApproveStageMain);
            command.Parameters.AddWithValue(command, "@ComparisonDate", entity.ComparisonDate);
            command.Parameters.AddWithValue(command, "@Comparisondeadline", entity.ComparisonDeadline);
            command.Parameters.AddWithValue(command, "@SpecialistComment", entity.SpecialistComment);
            command.Parameters.AddTableValue(command, "@SingleSourceReasonId", "SingleIdItems",
                entity.SingleSourceReasonId.ConvertListToDataTable());

            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            command.Parameters.Add("@NewBidComparisonId", SqlDbType.Int);
            command.Parameters["@NewBidComparisonId"].Direction = ParameterDirection.Output;

            using var reader = await command.ExecuteReaderAsync();
            int bidComparisonId = 0;
            if (reader.Read())
            {
                bidComparisonId = reader.Get<int>("NewBidComparisonId");
            }
            return bidComparisonId;
        }

        public async Task<bool> SaveComparisonBids(int bidComparisonId, DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_BidComparisonBids_IUD @TVP, @BidComparisonId";

            command.Parameters.AddWithValue(command, "@BidComparisonId", bidComparisonId == 0 ? null : bidComparisonId);
            command.Parameters.AddTableValue(command, "@TVP", "BidComparisonBidsType2", dataTable);
            var value = await command.ExecuteNonQueryAsync();

            return value > 0;
        }

        public async Task<bool> ApproveComparisonAsync(BidComparisonApprove entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonApprove @BidMainId,
                                        @Sequence,
                                        @ApproveStatus,
                                        @RFQDeatilid,
                                        @UserId,
                                        @Comment,
                                        @RejectReasonId";


            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);

            command.Parameters.AddWithValue(command, "@Sequence", entity.Sequence);

            command.Parameters.AddWithValue(command, "@ApproveStatus", entity.ApproveStatus);

            command.Parameters.AddWithValue(command, "@RFQDeatilid", entity.RFQDeatilid);

            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);

            command.Parameters.AddWithValue(command, "@Comment", entity.Comment);

            command.Parameters.AddWithValue(command, "@RejectReasonId", entity.RejectReasonId);

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

		public async Task<bool> BidApprove(BidComparisonBidApproveDto dto, int UserId)
		{
			using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText = "EXEC SP_BidComparisonBidsApprove @BidComparisonId, @approveStatus, @UserId, @Comment, @RejectReasonId";

			command.Parameters.AddWithValue(command, "@BidComparisonId", dto.BidComparisonId);
			command.Parameters.AddWithValue(command, "@approveStatus", dto.ApproveStatusId);
            command.Parameters.AddWithValue(command, "@UserId", UserId);
            command.Parameters.AddWithValue(command, "@Comment", dto.Comment);
            command.Parameters.AddWithValue(command, "@RejectReasonId", dto.RejectReasonId);

			return await command.ExecuteNonQueryAsync() > 0;
		}

		public async Task BidReject(BidComparisonBidRejectDto dto, int UserId)
		{
            foreach (var BidComparisonBidId in dto.BidComparisonBidIds)
            {
                using (var command = _unitOfWork.CreateCommand() as SqlCommand)
                {
                    command.CommandText = "EXEC SP_BidComparisonBidsReject @BidComparisonBidId, @UserId";

                    command.Parameters.AddWithValue(command, "@BidComparisonBidId", BidComparisonBidId);
                    command.Parameters.AddWithValue(command, "@UserId", UserId);
                        
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

		public async Task<List<BidComparisonAll>> GetComparisonAll(BidComparisonAllFilter filter)
        {
            var data = new List<BidComparisonAll>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_BidComparisonAll @BusinessUnitid,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @Status,
                                        @ApproveStatus";


                command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);

                command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);

                command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);

                command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);

                command.Parameters.AddWithValue(command, "@Status", filter.Status);

                command.Parameters.AddWithValue(command, "@ApproveStatus", filter.ApproveStatus);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonAll>());
                }
            }

            return data;
        }

        public async Task<List<BidComparisonBidApprovalsLoad>> GetComparisonBidApprovals(
            BidComparisonBidApprovalsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidApprovalsLoad>();
            command.CommandText = "EXEC SP_BidComparisonBidApprovalsLoad @RFQMainId, @UserId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
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
                    BidDetailId = reader.Get<int>("BidDetailId"),
                    RFQDetailId = reader.Get<int>("RFQDetailId")
                });
            }

            return data;
        }

        public async Task<List<BidComparisonApprovalInformationLoad>> GetComparisonApprovalInformations(
            BidComparisonApprovalInformationFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonApprovalInformationLoad>();
            command.CommandText = "EXEC SP_BidComparisonApprovalInformationLoad @RFQMainId";
            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonApprovalInformationLoad
                {
                    ApproveDate = reader.Get<DateTime>("ApproveDate"),
                    ApproveStageDetailsName = reader.Get<string>("ApproveStageDetailsName"),
                    ApproveStatus = reader.Get<string>("ApproveStatus"),
                    Comment = reader.Get<string>("Comment"),
                    FullName = reader.Get<string>("FullName"),
                    Sequence = reader.Get<int>("Sequence"),
                    SignaturePhoto = reader.Get<string>("SignaturePhoto"),
                    UserPhoto = reader.Get<string>("UserPhoto")
                });
            }

            return data;
        }

        public async Task<List<BidComparisonBidDetailsLoad>> GetComparisonBidDetails(
            BidComparisonBidDetailsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidDetailsLoad>();
            command.CommandText = "EXEC SP_BidComparisonBidDetailsLoad @RFQMainId, @UserId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
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
                    RFQDetailId = reader.Get<int>("RFQDetailId"),
                    BaseTotalWithRate = reader.Get<decimal>("BaseTotalWithRate"),
                    ConvertedGross = reader.Get<decimal>("ConvertedGross"),
                    Margins = reader.Get<decimal>("Margins")
                });
                ;
            }

            return data;
        }

        public async Task<List<BidComparisonRFQDetailsLoad>> GetComparisonRFQDetails(
            BidComparisonRFQDetailsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonRFQDetailsLoad>();
            command.CommandText = "EXEC SP_BidComparisonRFQDeatilsLoad @RFQMainId, @UserId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonRFQDetailsLoad
                {
                    LineNo = reader.Get<int>("LineNo"),
                    Budget = reader.Get<decimal>("Budget"),
                    Quantity = reader.Get<decimal>("Quantity"),
                    Description = reader.Get<string>("Description"),
                    RemainingBudget = reader.Get<decimal>("RemainingBudget"),
                    UOM = reader.Get<string>("UOM"),
                    RFQDetailId = reader.Get<int>("RFQDetailId"),
                    Brand = reader.Get<string>("Brand"),
                    CurrentPrice = reader.Get<decimal>("CurrentPrice")
                });
            }

            return data;
        }

        public async Task<List<BidComparisonBidHeaderLoad>> GetComparisonBidHeader(BidComparisonBidHeaderFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidHeaderLoad>();
            command.CommandText = "EXEC SP_BidComparisonBidHeaderLoad @RFQMainId, @UserId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonBidHeaderLoad
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
                    VendorName = reader.Get<string>("VendorName"),
                    BaseTotalWithRate = reader.Get<decimal>("BaseTotalWithRate"),
                    ConvertedGross = reader.Get<decimal>("ConvertedGross"),
                    Margins = reader.Get<decimal>("Margins")
                });
            }

            return data;
        }

        public async Task<List<BidComparisonBidDto>> GetComparisonBidsLoad(BidComparisonBidHeaderFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonBidDto>();
            command.CommandText = "EXEC SP_BidComparisonBidsLoad @BidcomparisonId, @RFQMainId";

            var bidComparisonId = filter.BidComparisonId == 0 ? null : filter.BidComparisonId;

            command.Parameters.AddWithValue(command, "@BidcomparisonId", bidComparisonId);
            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonBidDto
                {
                    LineNo = reader.Get<int>("LineNo"),
                    Quantity = reader.Get<decimal>("Quantity"),
                    UOM = reader.Get<string>("UOM"),
                    RFQItem = reader.Get<string>("RFQItem"),
                    VendorName = reader.Get<string>("VendorName"),
                    BidItem = reader.Get<string>("BidItem"),
                    BidItemName = reader.Get<string>("ItemName"),
                    SelectedQTY = reader.Get<decimal>("SelectedQTY"),
                    BidQuantity = reader.Get<decimal>("BidQuantity"),
                    PUOM = reader.Get<string>("PUOM"),
                    CurrencyCode = reader.Get<string>("CurrencyCode"),
                    UnitPrice = reader.Get<decimal>("UnitPrice"),
                    DiscountedAmount = reader.Get<decimal>("DiscountedAmount"),
                    ReportingAmount = reader.Get<decimal>("ReportingAmount"),
                    DeliverytermName = reader.Get<string>("DeliverytermName"),
                    DeliveryTime = reader.Get<string>("DeliveryTime"),
                    PaymentTermName = reader.Get<string>("PaymentTermName"),
                    Score = reader.Get<decimal>("Score"),
                    BidDetailId = reader.Get<int>("BidDetailId"),
                    RfqDetailId = reader.Get<int>("RfqDetailid"),
                    BidComparisonBidId = reader.Get<int>("BidComparisonBidId"),
                    IsSelected = reader.Get<bool>("IsSelected")
                });
            }

            return data;
        }

        public async Task<BidComparisonHeaderLoad> GetComparisonHeader(BidComparisonHeaderFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonHeaderLoad @RFQMainId, @UserId, @BidComparisonId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);
            command.Parameters.AddWithValue(command, "@BidComparisonId", filter.BidComparisonId);

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
                    Comparisondeadline = reader.Get<DateTime>("Comparisondeadline"),
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
                    Status = reader.Get<int>("Status"),
                    SentDate = reader.Get<DateTime>("SentDate")
                };
            }

            return null;
        }

        public async Task<List<BidComparisonSingleSourceReasonsLoad>> GetComparisonSingleSourceReasons(
            BidComparisonSingleSourceReasonsFilter filter)
        {
            var data = new List<BidComparisonSingleSourceReasonsLoad>();

            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonSingleSourceReasonsLoad @RFQMainId";

            command.Parameters.AddWithValue(command, "@RFQMainId", filter.RFQMainId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonSingleSourceReasonsLoad
                {
                    Checked = reader.Get<bool>("Checked"),
                    Other = reader.Get<int>("Other"),
                    SingleSourceReason = reader.Get<string>("SingleSourceReason"),
                    SingleSourcereasonId = reader.Get<int>("SingleSourcereasonId"),
                });
            }

            return data;
        }


        public async Task<List<BidComparisonWFALoad>> GetComparisonWFA(BidComparisonWFAFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonWFALoad @BusinessUnitid,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo,
                                        @UserId";

            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);

            var data = new List<BidComparisonWFALoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var child = GetBaseComparisonFromReader(reader).GetChild<BidComparisonWFALoad>();
                child.Sequence = reader.Get<int>("Sequence");
                data.Add(child);
            }

            return data;
        }

        public async Task<List<BidComparisonDraftLoad>> GetComparisonDraft(BidComparisonDraftFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonDraftLoad @BusinessUnitid,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo";

            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);

            var data = new List<BidComparisonDraftLoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonDraftLoad>());
            }

            return data;
        }

        public async Task<List<BidComparisonHeldLoad>> GetComparisonHeld(BidComparisonHeldFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonHeldLoad @BusinessUnitid,
                                        @Emergency,
                                        @DateFrom,
                                        @DateTo";


            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);

            var data = new List<BidComparisonHeldLoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonHeldLoad>());
            }

            return data;
        }

        public async Task<List<BidComparisonMyChartsLoad>> GetComparisonMyCharts(BidComparisonMyChartsFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonMyChartsLoad @BusinessUnitid,
                                    @Emergency,
                                    @DateFrom,
                                    @DateTo,
                                    @UserId,
                                    @Status,
                                    @ApproveStatus";


            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
            command.Parameters.AddWithValue(command, "@UserId", filter.UserId);
            command.Parameters.AddWithValue(command, "@Status", filter.Status);
            command.Parameters.AddWithValue(command, "@ApproveStatus", filter.ApproveStatus);

            var data = new List<BidComparisonMyChartsLoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonMyChartsLoad>());
            }

            return data;
        }

        public async Task<List<BidComparisonNotReleasedLoad>> GetComparisonNotReleased(
            BidComparisonNotReleasedFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonNotReleasedLoad @BusinessUnitid,
                                    @Emergency,
                                    @DateFrom,
                                    @DateTo";

            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);

            var data = new List<BidComparisonNotReleasedLoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonNotReleasedLoad>());
            }

            return data;
        }

        public async Task<List<BidComparisonRejectedLoad>> GetComparisonRejected(BidComparisonRejectedFilter filter)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonRejectedLoad @BusinessUnitid,
                                    @Emergency,
                                    @DateFrom,
                                    @DateTo";

            command.Parameters.AddWithValue(command, "@BusinessUnitid", filter.BusinessUnitid);
            command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
            command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);

            var data = new List<BidComparisonRejectedLoad>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(GetBaseComparisonFromReader(reader).GetChild<BidComparisonRejectedLoad>());
            }

            return data;
        }

        public async Task<bool> HoldBidComparison(HoldBidComparisonRequest request)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_BidComparisonHold @BidComparisonId, @Sequence, @UserId, @Comment";

            command.Parameters.AddWithValue(command, "@BidComparisonId", request.BidComparisonId);
            command.Parameters.AddWithValue(command, "@Sequence", request.Sequence);
            command.Parameters.AddWithValue(command, "@UserId", request.UserId);
            command.Parameters.AddWithValue(command, "@Comment", request.Comment);

            await _unitOfWork.SaveChangesAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }


        private BaseBidComparisonLoad GetBaseComparisonFromReader(DbDataReader reader)
        {
            return new BaseBidComparisonLoad
            {
                BidComparisonId = reader.Get<int>("BidComparisonId"),
                RFQMainId = reader.Get<int>("RFQMainId"),
                ApproveStatus = reader.Get<int>("ApproveStatus"),
                Buyer = reader.Get<string>("Buyer"),
                Comparisondeadline = reader.Get<DateTime?>("Comparisondeadline"),
                ComparisonNo = reader.Get<string>("ComparisonNo"),
                Emergency = reader.Get<int>("Emergency"),
                ProcurementType = reader.Get<int>("ProcurementType"),
                RFQDeadline = reader.Get<DateTime?>("RFQDeadline"),
                RFQNo = reader.Get<string>("RFQNo"),
                RowNum = reader.Get<long>("RowNum"),
                SingleSourceReasons = reader.Get<string>("SingleSourceReasons"),
                SpecialistComment = reader.Get<string>("SpecialistComment"),
                CreatedBy = reader.Get<string>("CreatedBy"),
                ComparisonDate = reader.Get<DateTime?>("ComparisonDate"),
                HasAttachments = reader.Get<bool>("HasAttachments"),
                Status = reader.Get<int>("Status"),
                StatusName = reader.Get<string>("StatusName")
            };
        }

        public async Task<bool> OrderCreateFromApproveBid(CreateOrderFromBidDto entity)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_OrderCreateFromApprovedBid @BidMainId,@UserId";

            command.Parameters.AddWithValue(command, "@BidMainId", entity.BidMainId);
            command.Parameters.AddWithValue(command, "@UserId", entity.UserId);


            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BidComparisonSummarySave(DataTable dataTable)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_BidComparisonSummary_IUD @tvp";
                command.Parameters.AddTableValue(command, "@tvp", "BidComparisonSummaryType", dataTable);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<List<BidComparisonSummary>> BidComparisonSummaryLoad(int bidComparisonId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            var data = new List<BidComparisonSummary>();
            command.CommandText = "EXEC SP_BidComparisonSummaryLoad @bidComparisonId";

            command.Parameters.AddWithValue(command, "@bidComparisonId", bidComparisonId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonSummary
                {
                    BidComparisonId = reader.Get<int>("BidComparisonId"),
                    BidComparisonSummaryId = reader.Get<int>("BidComparisonSummaryId"),
                    NormalizationCost = reader.Get<decimal>("NormalizationCost"),
                    VendorCode = reader.Get<string>("VendorCode"),
                });
            }

            return data;
        }
        
        public async Task<List<BidComparisonApprovalInfoDto>> BidComparisonApprovalInfo(int bidComparisonId)
        {
            var data = new List<BidComparisonApprovalInfoDto>();

            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC SP_BidComparisonBidsApprovalInfo @bidComparisonId";

            command.Parameters.AddWithValue(command, "@bidComparisonId", bidComparisonId);

            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                data.Add(new BidComparisonApprovalInfoDto
                {
                    Sequence = reader.Get<int>("Sequence"),
                    ApproveStageDetailsName = reader.Get<string>("ApproveStageDetailsName"),
                    FullName = reader.Get<string>("FullName"),
                    ApproveStatus = reader.Get<string>("ApproveStatus"),
                    ApproveDate = reader.Get<DateTime>("ApproveDate"),
                    Comment = reader.Get<string>("Comment"),
                    ReasonName = reader.Get<string>("ReasonName"),
                    SignaturePhoto = reader.Get<string>("SignaturePhoto"),
                });
            }
            
            return data.Count > 0 ? data : null;
        }
    }
}
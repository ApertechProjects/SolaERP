using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlRfqRepository : IRfqRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlRfqRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_RFQDraft @BusinessUnitId,
                                                         @ItemCode,
                                                         @Emergency,
                                                         @DateFrom,
                                                         @DateTo,
                                                         @RFQType,
                                                         @ProcurementType";


                command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", filter.ItemCode);
                command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
                command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
                command.Parameters.AddWithValue(command, "@RFQType", filter.RFQType);
                command.Parameters.AddWithValue(command, "@ProcurementType", filter.ProcurementType);

                List<RfqDraft> rfqDrafts = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqDrafts.Add(GetRfqBaseFromReader(reader) as RfqDraft);
                return rfqDrafts;
            }
        }



        #region Readers 
        private RFQBase GetRfqBaseFromReader(IDataReader reader)
        {
            return new()
            {
                RFQMainId = reader.Get<int>("RFQMainId"),
                RequiredOnSiteDate = reader.Get<DateTime>("RequiredOnSiteDate"),
                Emergency = reader.Get<string>("Emergency"),
                RFQDate = reader.Get<DateTime>("RFQDate"),
                RFQType = reader.Get<string>("RFQType"),
                RFQNo = reader.Get<int>("RFQNo"),
                DesiredDeliveryDate = reader.Get<DateTime>("DesiredDeliveryDate"),
                ProcurementType = reader.Get<string>("ProcurementType"),
                OtherReasons = reader.Get<string>("OtherReasons"),
                SentDate = reader.Get<DateTime>("SentDate"),
                Comment = reader.Get<string>("Comment"),
                RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                SingleUnitPrice = reader.Get<bool>("SingleUnitPrice"),
                PlaceOfDelivery = reader.Get<string>("PlaceOfDelivery"),
                BusinessCategoryid = reader.Get<int>("BusinessCategoryid")
            };
        }

        private RfqAll GetRfqAllFromReader(IDataReader reader)
        {
            var rfqAll = this.GetRfqBaseFromReader(reader) as RfqAll;
            rfqAll.Status = reader.Get<string>("Status");

            return rfqAll;
        }

        public async Task<List<RfqAll>> GetAllAsync(RfqAllFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_RFQAll @BusinessUnitId,
                                                       @ItemCode,
                                                       @Emergency,
                                                       @DateFrom,
                                                       @DateTo,
                                                       @RFQType,
                                                       @Status,
                                                       @ProcurementType";


                command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", filter.ItemCode);
                command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
                command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
                command.Parameters.AddWithValue(command, "@RFQType", filter.RFQType);
                command.Parameters.AddWithValue(command, "@Status", filter.Status);
                command.Parameters.AddWithValue(command, "@ProcurementType", filter.ProcurementType);

                List<RfqAll> rfqAlls = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqAlls.Add(GetRfqAllFromReader(reader));
                return rfqAlls;
            }
        }

        public async Task<int> AddMainAsync(RfqSaveCommandRequest request)
        {
            return await ModifyRfqMainAsync(new()
            {
                RFQMainId = 0,
                BusinessUnitId = request.BusinessUnitId,
                RFQType = request.RFQType,
                RFQNo = request.RFQNo,
                Emergency = request.Emergency,
                RFQDate = request.RFQDate,
                RFQDeadline = request.RFQDeadline,
                Buyer = request.Buyer,
                Status = request.Status,
                RequiredOnSiteDate = request.RequiredOnSiteDate,
                DesiredDeliveryDate = request.DesiredDeliveryDate,
                SentDate = request.SentDate,
                SingleUnitPrice = request.SingleUnitPrice,
                ProcurementType = request.ProcurementType,
                SingleSourceReasonIds = request.SingleSourceReasonIds,
                PlaceOfDelivery = request.PlaceOfDelivery,
                Comment = request.Comment,
                OtherReasons = request.OtherReasons,
                BusinessCategoryId = request.BusinessCategoryId,
                UserId = request.UserId,
            });
        }

        public async Task<int> UpdateMainAsync(RfqSaveCommandRequest request)
            => await ModifyRfqMainAsync(request);

        public async Task<int> DeleteMainsync(int id, int userId)
            => await ModifyRfqMainAsync(new RfqSaveCommandRequest() { RFQMainId = id, UserId = userId });


        private async Task<int> ModifyRfqMainAsync(RfqSaveCommandRequest request)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF
                                        DECLARE @NewRFQMainId int
                                        EXEC SP_RFQMain_IUD @RFQMainId,
                                                            @BusinessUnitId,
                                                            @RFQType,
                                                            @RFQNo,
                                                            @Emergency,
                                                            @RFQDate,
                                                            @RFQDeadline,
                                                            @Buyer,
                                                            @Status,
                                                            @RequiredOnSiteDate,
                                                            @DesiredDeliveryDate,
                                                            @SentDate,
                                                            @SingleUnitPrice,
                                                            @ProcurementType,
                                                            @SingleSourceReasonId,
                                                            @PlaceOfDelivery,
                                                            @Comment,
                                                            @OtherReasons,
                                                            @BusinessCategoryId,
                                                            @UserId,
                                                            @NewRFQMainId = @NewRFQMainId OUTPUT
                                                            SELECT @NewRFQMainId as [@NewRFQMainId]";


                command.Parameters.AddWithValue(command, "@RFQMainId", request.RFQMainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", request.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@RFQType", request.RFQType);
                command.Parameters.AddWithValue(command, "@RFQNo", request.RFQNo);
                command.Parameters.AddWithValue(command, "@Emergency", request.Emergency);
                command.Parameters.AddWithValue(command, "@RFQDate", request.RFQDate);
                command.Parameters.AddWithValue(command, "@RFQDeadline", request.RFQDeadline);
                command.Parameters.AddWithValue(command, "@Buyer", request.Buyer);
                command.Parameters.AddWithValue(command, "@Status", request.Status);
                command.Parameters.AddWithValue(command, "@RequiredOnSiteDate", request.RequiredOnSiteDate);
                command.Parameters.AddWithValue(command, "@DesiredDeliveryDate", request.DesiredDeliveryDate);
                command.Parameters.AddWithValue(command, "@SentDate", request.SentDate);
                command.Parameters.AddWithValue(command, "@SingleUnitPrice", request.SingleUnitPrice);
                command.Parameters.AddWithValue(command, "@ProcurementType", request.ProcurementType);
                command.Parameters.AddWithValue(command, "@PlaceOfDelivery", request.PlaceOfDelivery);
                command.Parameters.AddWithValue(command, "@Comment", request.Comment);
                command.Parameters.AddWithValue(command, "@OtherReasons", request.OtherReasons);
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", request.BusinessCategoryId);
                command.Parameters.AddWithValue(command, "@UserId", request.UserId);

                command.Parameters.Add("@SingleSourceReasonId", SqlDbType.Structured).Value = request.SingleSourceReasonIds.ConvertListToDataTable();
                command.Parameters["@SingleSourceReasonId"].TypeName = "SingleIdItems";


                using var reader = await command.ExecuteReaderAsync();
                int newRfqMainId = -1;

                if (reader.Read()) newRfqMainId = reader.Get<int>("@NewRFQMainId");

                return newRfqMainId;
            }
        }

        public async Task<List<SingleSourceReasonModel>> GetSingleSourceReasonsAsync()
        {
            using (var commmand = _unitOfWork.CreateCommand() as DbCommand)
            {
                commmand.CommandText = "SELECT * FROM VW_SingleSourcereason";
                List<SingleSourceReasonModel> singleSourceReasons = new();

                using var reader = await commmand.ExecuteReaderAsync();
                while (reader.Read()) singleSourceReasons.Add(GetReasonsFromReader(reader));

                return singleSourceReasons;
            }
        }


        private SingleSourceReasonModel GetReasonsFromReader(IDataReader reader)
        {
            return new()
            {
                Id = reader.Get<int>("SingleSourceReasonId"),
                SingleSourceReason = reader.Get<string>("SingleSourceReason"),
                Other = reader.Get<int>("Other")
            };
        }

        public Task<bool> AddDetailsAsync(RfqDetailsSaveRequestModel model)
            => ModifyRfqDetailAsync(new()
            {
                RFQMainId = model.RFQMainId,
                Details = model.Details
            });


        public Task<bool> UpdateDetailsAsync(RfqDetailsSaveRequestModel model)
            => ModifyRfqDetailAsync(model);

        public Task<bool> DeleteDetailsAsync(int detailId)
            => ModifyRfqDetailAsync(new() { RFQMainId = detailId });



        private async Task<bool> ModifyRfqDetailAsync(RfqDetailsSaveRequestModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQDetails_IUD @RFQMainId,@Data";


                command.Parameters.AddWithValue(command, "@RFQMainId", model.RFQMainId);
                command.Parameters.AddTableValue(command, "RFQDetailsType", model.Details.ConvertToDataTable());

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }


        #endregion

    }
}

using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.UOM;
using SolaERP.Application.Enums;
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

        public async Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_RFQDraft @BusinessUnitId,
                                                         @ItemCode,
                                                         @Emergency,
                                                         @DateFrom,
                                                         @DateTo,
                                                         @RFQType,
                                                         @ProcurementType,
                                                         @UserId";


                command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", filter.ItemCode);
                command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
                command.Parameters.AddWithValue(command, "@DateFrom", filter.DateFrom);
                command.Parameters.AddWithValue(command, "@DateTo", filter.DateTo);
                command.Parameters.AddWithValue(command, "@RFQType", filter.RFQType);
                command.Parameters.AddWithValue(command, "@ProcurementType", filter.ProcurementType);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                List<RfqDraft> rfqDrafts = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqDrafts.Add(GetRFQDraftFromReader(reader));
                return rfqDrafts;
            }
        }


        #region Readers

        private void PopulateRfqBaseFromReader(RFQBase rfqBase, IDataReader reader)
        {
            rfqBase.RFQMainId = reader.Get<int>("RFQMainId");
            rfqBase.LineNo = reader.Get<long>("LineNo");
            rfqBase.RequiredOnSiteDate = reader.Get<DateTime>("RequiredOnSiteDate");
            rfqBase.Emergency = reader.Get<string>("Emergency");
            rfqBase.RFQDate = reader.Get<DateTime?>("RFQDate");
            rfqBase.RFQType = reader.Get<string>("RFQType");
            rfqBase.RFQNo = reader.Get<string>("RFQNo");
            rfqBase.DesiredDeliveryDate = reader.Get<DateTime>("DesiredDeliveryDate");
            rfqBase.ProcurementType = reader.Get<string>("ProcurementType");
            rfqBase.OtherReasons = reader.Get<string>("OtherReasons");
            rfqBase.SentDate = reader.Get<DateTime?>("SentDate");
            rfqBase.Comment = reader.Get<string>("Comment");
            rfqBase.RFQDeadline = reader.Get<DateTime?>("RFQDeadline");
            rfqBase.Buyer = reader.Get<string>("Buyer");
            rfqBase.SingleUnitPrice = reader.Get<bool>("SingleUnitPrice");
            rfqBase.PlaceOfDelivery = reader.Get<string>("PlaceOfDelivery");
            rfqBase.BusinessCategoryId = reader.Get<int>("BusinessCategoryId");
            rfqBase.EntryDate = reader.Get<DateTime>("EnteredDate");
            try
            {
                rfqBase.EnteredBy = reader.Get<string>("EnteredBy");
            }
            catch (Exception)
            {
                rfqBase.EnteredBy = null;
            }

            try
            {
                rfqBase.BiddingType = reader.Get<int>("BiddingType");
            }
            catch (Exception)
            {
                rfqBase.BiddingType = 0;
            }

            try
            {
                rfqBase.HasAttachments = reader.Get<bool>("HasAttachments");
            }
            catch (Exception)
            {
                rfqBase.HasAttachments = false;
            }

            rfqBase.BusinessCategory = new()
            {
                Id = reader.Get<int>("BusinessCategoryId"),
                Name = reader.Get<string>("BusinessCategoryName")
            };
        }

        private RfqDraft GetRFQDraftFromReader(IDataReader reader)
        {
            RfqDraft rfqDrafts = new();
            PopulateRfqBaseFromReader(rfqDrafts, reader);

            return rfqDrafts;
        }


        private RfqAll GetRfqAllFromReader(IDataReader reader)
        {
            RfqAll rfqAll = new RfqAll();
            PopulateRfqBaseFromReader(rfqAll, reader);
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

        public async Task<RfqSaveCommandResponse> AddMainAsync(RfqSaveCommandRequest request)
        {
            return await ModifyRfqMainAsync(new()
            {
                Id = 0,
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
                BiddingType = request.BiddingType
            });
        }

        public async Task<RfqSaveCommandResponse> UpdateMainAsync(RfqSaveCommandRequest request)
            => await ModifyRfqMainAsync(request);

        public async Task<RfqSaveCommandResponse> DeleteMainsync(int id, int userId)
            => await ModifyRfqMainAsync(new RfqSaveCommandRequest() { Id = id, UserId = userId });

        private async Task<RfqSaveCommandResponse> ModifyRfqMainAsync(RfqSaveCommandRequest request)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"DECLARE @NewRFQMainId INT,@NewRFQNo NVARCHAR(25)
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
                                                            @BiddingType,
                                                            @NewRFQMainId = @NewRFQMainId OUTPUT,
                                                            @NewRFQNo = @NewRFQNo OUTPUT
                                                            
                                                            SELECT	@NewRFQMainId as N'@NewRFQMainId',
		                                                            @NewRFQNo as N'@NewRFQNo'";


                command.Parameters.AddWithValue(command, "@RFQMainId", request.Id);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", request.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@RFQType", request?.RFQType);
                command.Parameters.AddWithValue(command, "@RFQNo", request?.RFQNo);
                command.Parameters.AddWithValue(command, "@Emergency", request?.Emergency);
                command.Parameters.AddWithValue(command, "@RFQDate", request?.RFQDate);
                command.Parameters.AddWithValue(command, "@RFQDeadline", request?.RFQDeadline);
                command.Parameters.AddWithValue(command, "@Buyer", request?.Buyer);
                command.Parameters.AddWithValue(command, "@Status", request?.Status);
                command.Parameters.AddWithValue(command, "@RequiredOnSiteDate", request?.RequiredOnSiteDate);
                command.Parameters.AddWithValue(command, "@DesiredDeliveryDate", request?.DesiredDeliveryDate);
                command.Parameters.AddWithValue(command, "@SentDate", request?.SentDate);
                command.Parameters.AddWithValue(command, "@SingleUnitPrice", request?.SingleUnitPrice);
                command.Parameters.AddWithValue(command, "@ProcurementType", request?.ProcurementType);
                command.Parameters.AddWithValue(command, "@PlaceOfDelivery", request?.PlaceOfDelivery);
                command.Parameters.AddWithValue(command, "@Comment", request?.Comment);
                command.Parameters.AddWithValue(command, "@OtherReasons", request?.OtherReasons);
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", request?.BusinessCategoryId);
                command.Parameters.AddWithValue(command, "@UserId", request?.UserId);
                command.Parameters.AddWithValue(command, "@BiddingType", request?.BiddingType);
                command.Parameters.AddTableValue(command, "@SingleSourceReasonId", "SingleIdItems",
                    request?.SingleSourceReasonIds?.ConvertListToDataTable());

                using var reader = await command.ExecuteReaderAsync();
                RfqSaveCommandResponse response = null;

                if (reader.Read()) response = GetRfqSaveResponse(reader);
                return response;
            }
        }


        private RfqSaveCommandResponse GetRfqSaveResponse(IDataReader reader)
        {
            return new()
            {
                Id = reader.Get<int>("@NewRFQMainId"),
                RfqNo = reader.Get<string>("@NewRFQNo"),
            };
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

        //public async Task<bool> DeleteDetailAsync(List<RfqDetailSaveModel> details, int rfqMainId)
        //    => await ModifyRfqDetailAsync(details, rfqMainId);

        public async Task<bool> DetailsIUDAsync(List<RfqDetailSaveModel> details, int rfqMainId)
            => await ModifyRfqDetailAsync(details, rfqMainId);


        private async Task<bool> ModifyRfqDetailAsync(List<RfqDetailSaveModel> details, int? mainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQDetails_IUD @RFQMainId,@Data";

                command.Parameters.AddWithValue(command, "@RFQMainId", mainId);
                command.Parameters.AddTableValue(command, "@Data", "dbo.RFQDetailsType2",
                    details?.ConvertToDataTable());
                var res = await command.ExecuteNonQueryAsync();
                return res > 0;
            }
        }


        public async Task<List<RequestForRFQ>> GetRequestsForRfq(RFQRequestModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    @"EXEC SP_RFQCreateRequestList @BusinessUnitId,@Businesscategoryid,@Buyer,@UserId";

                command.Parameters.AddWithValue(command, "@BusinessUnitId", model?.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@Businesscategoryid",
                    string.Join(",", model?.BusinessCategoryIds));
                command.Parameters.AddWithValue(command, "@Buyer", string.Join(",", model?.Buyer));
                command.Parameters.AddWithValue(command, "@UserId", model.UserId);

                using var reader = await command.ExecuteReaderAsync();
                List<RequestForRFQ> requestListForRfq = new();

                while (reader.Read()) requestListForRfq.Add(reader.GetByEntityStructure<RequestForRFQ>());
                return requestListForRfq;
            }
        }

        public async Task<bool> RFQRequestDetailsIUDAsync(List<RfqRequestDetailSaveModel> details)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_RFQRequestDetails_IUD @Data";
                command.Parameters.AddTableValue(command, "@Data", "RFQRequestDetailsType2",
                    details?.ConvertToDataTable());

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RfqVendorToSend>> GetVendorsForRfqAync(int businessCategoryId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQVendorsToSend @BusinessCategoryId";
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", businessCategoryId);

                List<RfqVendorToSend> rfqVendors = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqVendors.Add(reader.GetByEntityStructure<RfqVendorToSend>());
                return rfqVendors;
            }
        }

        public async Task<bool> ChangeRFQStatusAsync(RfqChangeStatusModel model, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQChangeStatus @RFQMainId,@UserId,@Status";


                command.Parameters.AddWithValue(command, "@RFQMainId", model.Id);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Status", model.Status);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RFQSingleSourceReasonsLoad>> GetSingleSourceReasons(int rfqMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQSingleSourceReasonsLoad @RFQMainId";

                command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);
                List<RFQSingleSourceReasonsLoad> data = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) data.Add(reader.GetByEntityStructure<RFQSingleSourceReasonsLoad>());
                return data;
            }
        }

        public async Task<RFQMain> GetRFQMainAsync(int rfqMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQRequestHeaderLoad @RFQMainId";
                command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);

                RFQMain rfqMain = null;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read()) rfqMain = GetRFQMainFromReader(reader);
                return rfqMain;
            }
        }

        private RFQMain GetRFQMainFromReader(IDataReader reader)
        {
            return new()
            {
                Id = reader.Get<int>("RFQMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RFQType = (RfqType)reader.Get<int>("RFQType"),
                RFQNo = reader.Get<string>("RFQNo"),
                Emergency = (Emergency)reader.Get<int>("Emergency"),
                RFQDate = reader.Get<DateTime?>("RFQDate"),
                RFQDeadline = reader.Get<DateTime?>("RFQDeadline"),
                Buyer = reader.Get<string>("Buyer"),
                Status = (Status)reader.Get<int>("Status"),
                RequiredOnSiteDate = reader.Get<DateTime?>("RequiredOnSiteDate"),
                DesiredDeliveryDate = reader.Get<DateTime?>("DesiredDeliveryDate"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                SingleUnitPrice = reader.Get<bool>("SingleUnitPrice"),
                ProcurementType = (ProcurementType)reader.Get<int>("ProcurementType"),
                PlaceOfDelivery = reader.Get<string>("PlaceOfDelivery"),
                Comment = reader.Get<string>("Comment"),
                OtherReasons = reader.Get<string>("OtherReasons"),
                EnteredBy = reader.Get<string>("EnteredBy"),
                BusinessCategoryId = reader.Get<int>("BusinessCategoryId"),
                BiddingType = (BiddingType)reader.Get<int>("BiddingType"),
                EntryDate = reader.Get<DateTime>("EnteredDate")
            };
        }


        public async Task<List<RFQDetail>> GetRFQDetailsAsync(int rfqMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQDetailsLoad @RFQMainId";
                command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);

                List<RFQDetail> rfqDetails = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqDetails.Add(GetRFQDetailFromReader(reader));
                return rfqDetails;
            }
        }


        private RFQDetail GetRFQDetailFromReader(IDataReader reader)
        {
            return new()
            {
                Id = reader.Get<int>("RFQDetailId"),
                RFQMainId = reader.Get<int>("RFQMainId"),
                LineNo = reader.Get<int>("LineNo"),
                ItemCode = reader.Get<string>("ItemCode"),
                ItemName1 = reader.Get<string>("ItemName1"),
                ItemName2 = reader.Get<string>("ItemName2"),
                BusinessCategory = new Application.Entities.SupplierEvaluation.BusinessCategory
                {
                    Id = reader.Get<int>("BusinessCategoryId"),
                    Name = reader.Get<string>("BusinessCategoryName")
                },
                Condition = (Condition)reader.Get<int>("Condition"),
                AlternativeItem = reader.Get<bool>("AlternativeItems"),
                UOM = reader.Get<string>("UOM"),
                DefaultUOM = reader.Get<string>("DefaultUOM"),
                Quantity = reader.Get<decimal>("Quantity"),
                CONV_ID = reader.Get<decimal>("CONV_ID"),
                Description = reader.Get<string>("Description"),
                GUID = reader.Get<Guid>("GUID")
            };
        }

        public async Task<List<RFQRequestDetail>> GetRFQLineDeatilsAsync(int rfqMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQRequestDetailsLoad @RFQMainId";
                command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);

                List<RFQRequestDetail> rfqRequestDetails = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) rfqRequestDetails.Add(GetRFQRequestDetailFromReader(reader));
                return rfqRequestDetails;
            }
        }

        private RFQRequestDetail GetRFQRequestDetailFromReader(IDataReader reader)
        {
            return new()
            {
                //RowNum = reader.Get<long>("RowNum"),
                Id = reader.Get<int>("RFQRequestDetailId"),
                RFQDetailId = reader.Get<int>("RFQDetailId"),
                RequestDetailId = reader.Get<int>("RequestDetailsId"),
                RequestNo = reader.Get<string>("RequestNo"),
                RequestLine = reader.Get<string>("RequestLine"),
                RFQLine = reader.Get<int>("RFQLine"),
                ItemCode = reader.Get<string>("ItemCode"),
                ItemName1 = reader.Get<string>("ItemName1"),
                ItemName2 = reader.Get<string>("ItemName2"),
                Quantity = reader.Get<decimal>("Quantity"),
                UOM = reader.Get<string>("UOM"),
                DefaultUOM = reader.Get<string>("DefaultUOM"),
                CONV_ID = reader.Get<decimal>("CONV_ID"),
                GUID = reader.Get<Guid>("GUID"),
                Condition = (Condition)reader.Get<int>("Condition"),
                Buyer = reader.Get<string>("Buyer"),
                AlternativeItem = reader.Get<bool>("AlternativeItem"),
                RequestQuantity = reader.Get<decimal>("RequestQuantity"),
                BusinessCategory = new()
                {
                    Id = reader.Get<int>("BusinessCategoryId"),
                    Name = reader.Get<string>("BusinessCategoryName")
                },
            };
        }

        public async Task<List<SingleSourceReasonModel>> GetRFQSingeSourceReasons(int rfqMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_RFQSingleSourceReasonsLoad @RFQMainId";
                command.Parameters.AddWithValue(command, "@RFQMainId", rfqMainId);

                List<SingleSourceReasonModel> singleSourceReasons = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) singleSourceReasons.Add(GetReasonsFromReader(reader));
                return singleSourceReasons;
            }
        }

        public async Task<List<RFQInProgress>> GetInProgressesAsync(RFQFilterBase filter, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    @"EXEC SP_RFQInProgress @BusinessUnitId,@ItemCode,@Emergency,@RFQType,@ProcurementType,@UserId";


                command.Parameters.AddWithValue(command, "@BusinessUnitId", filter.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", filter.ItemCode);
                command.Parameters.AddWithValue(command, "@Emergency", filter.Emergency);
                command.Parameters.AddWithValue(command, "@RFQType", filter.RFQType);
                command.Parameters.AddWithValue(command, "@ProcurementType", filter.ProcurementType);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                List<RFQInProgress> inProgressRFQs = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) inProgressRFQs.Add(reader.GetByEntityStructure<RFQInProgress>());
                return inProgressRFQs;
            }
        }

        public async Task<List<Application.Entities.RFQ.UOM>> GetPUOMAsync(int businessUnitId, string itemCodes)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_UOMConvList @BusinessUnitId,@ItemCode";

                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCodes);

                List<Application.Entities.RFQ.UOM> list = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read()) list.Add(reader.GetByEntityStructure<Application.Entities.RFQ.UOM>());
                return list;
            }
        }

        public async Task<Application.Entities.UOM.Conversion> GetConversionAsync(int bussinessUnit, string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_UOMConvList  @BusinessUnitId,@ItemCode";

                Application.Entities.UOM.Conversion conversion = null;
                command.Parameters.AddWithValue(command, "@BusinessUnitId", bussinessUnit);
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    conversion = reader.GetByEntityStructure<Application.Entities.UOM.Conversion>();

                return conversion;
            }
        }

        public async Task<bool> RFQVendorIUDAsync(RFQVendorIUD vendorIUD, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQVendorResponse_IUD @RFQMainId,@VendorCode,@UserId";


                command.Parameters.AddWithValue(command, "@RFQMainId", vendorIUD.Id);
                command.Parameters.AddTableValue(command, "@VendorCode", "SingleNvarcharItems",
                    vendorIUD.VendorCodes.ConvertListToDataTable());
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        #endregion
    }
}
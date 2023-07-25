using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;


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

                while (reader.Read()) rfqDrafts.Add(GetRfqDraftFromReader(reader));
                return rfqDrafts;
            }
        }



        #region Readers 
        private RfqDraft GetRfqDraftFromReader(IDataReader reader)
        {
            return new()
            {
                RFQMainId = reader.Get<int>("RRFQMainId"),
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

        #endregion

    }
}

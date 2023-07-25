using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;

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


        #endregion

    }
}

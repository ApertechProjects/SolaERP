using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Helper;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlInvoiceRepository : IInvoiceRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BusinessUnitHelper _businessUnitHelper;

        public SqlInvoiceRepository(IUnitOfWork unitOfWork, BusinessUnitHelper businessUnitHelper)
        {
            _unitOfWork = unitOfWork;
            _businessUnitHelper = businessUnitHelper;
        }

        public async Task<bool> ChangeStatus(int invoiceRegisterId, int sequence, int approveStatus, string comment,
            int userId, int? rejectReasonId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_InvoiceRegisterApprove @invoiceRegisterId,@sequence,@approveStatus,@userId,@comment,@rejectReeasonId";

                command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);
                command.Parameters.AddWithValue(command, "@sequence", sequence);
                command.Parameters.AddWithValue(command, "@approveStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@comment", comment);
                command.Parameters.AddWithValue(command, "@rejectReeasonId", rejectReasonId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

		public async Task<bool> InvoiceApproveIntegration(int invoiceRegisterId, int userId, int businessUnitId)
		{
			await using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText = _businessUnitHelper.BuildQueryForIntegration(businessUnitId,
				"SP_InvoiceRegisterWOOrder_I @InvoiceRegisterId, @UserId");

			command.Parameters.AddWithValue(command, "@InvoiceRegisterId", invoiceRegisterId);
			command.Parameters.AddWithValue(command, "@UserId", userId);
			await _unitOfWork.SaveChangesAsync();
			return await command.ExecuteNonQueryAsync() > 0;
		}

        public async Task<List<RegisterAll>> RegisterAll(InvoiceRegisterGetModel model, int userId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterAll @businessUnitId,@dateFrom,@dateTo,@userId";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterAll> list = new List<RegisterAll>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterAll>());

            return list;
        }

        public async Task<List<RegisterListByOrder>> RegisterListByOrder(int orderMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterListByOrder @orderMainId";
            command.Parameters.AddWithValue(command, "@orderMainId", orderMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterListByOrder> list = new List<RegisterListByOrder>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterListByOrder>());

            return list;
        }

        public async Task<List<RegisterLoadGRN>> RegisterLoadGRN(int orderMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterLoadGRNs @OrderMainId";
            command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterLoadGRN> list = new List<RegisterLoadGRN>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterLoadGRN>());

            return list;
        }

        public async Task<RegisterMainLoad> RegisterMainLoad(int invoiceRegisterId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterLoadMain @invoiceRegisterId";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

            using var reader = await command.ExecuteReaderAsync();

            RegisterMainLoad data = new RegisterMainLoad();
            if (reader.Read())
                data = reader.GetByEntityStructure<RegisterMainLoad>();

            return data;
        }

        public async Task<bool> RegisterSendToApprove(int invoiceRegisterId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_InvoiceRegisterSendToApprove @invoiceRegisterId,@userId";

                command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<RegisterWFA>> RegisterWFA(InvoiceRegisterGetModel model, int userId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterWFA @businessUnitId,@dateFrom,@dateTo,@userId";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterWFA> list = new List<RegisterWFA>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterWFA>());

            return list;
        }

        public async Task<int> Save(InvoiceRegisterSaveModel model, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
					@"EXEC SP_InvoiceRegister_IUD @InvoiceRegisterId,
                                                  @BusinessUnitId,
                                                  @InvoiceType,
                                                  @InvoiceDate,
                                                  @InvoiceReceivedDate,
                                                  @InvoiceNo, 
                                                  @SystemInvoiceNo,
                                                  @OrderType,
                                                  @OrderMainId,
                                                  @ReferenceDocNo,
                                                  @InvoiceAmount,
                                                  @CurrencyCode, 
                                                  @LineDescription,
                                                  @VendorCode,
                                                  @DueDate,
                                                  @AgingDays,
                                                  @ProblematicInvoiceReasonId,
                                                  @ReasonAdditionalDescription,
                                                  @Comment,
                                                  @AccountCode,
                                                  @WithHoldingTaxId,
                                                  @TaxId,
                                                  @TaxAmount,
                                                  @GrossAmount,
                                                  @OrderReference,
                                                  @InvoicePeriod,
                                                  @VendorAccount,
                                                  @WithHoldingTaxAmount,
                                                  @TransactionDate,
                                                  @UserId,
                                                  @NewInvoiceRegisterId = @NewInvoiceRegisterId OUTPUT 
                                                  select @NewInvoiceRegisterId as NewInvoiceRegisterId";


                command.Parameters.AddWithValue(command, "@InvoiceRegisterId", model.InvoiceRegisterId);

                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);

                command.Parameters.AddWithValue(command, "@InvoiceType", model.InvoiceType);

                command.Parameters.AddWithValue(command, "@InvoiceDate", model.InvoiceDate);

                command.Parameters.AddWithValue(command, "@InvoiceReceivedDate", model.InvoiceReceivedDate);

                command.Parameters.AddWithValue(command, "@InvoiceNo", model.InvoiceNo);

                command.Parameters.AddWithValue(command, "@SystemInvoiceNo", model.SystemInvoiceNo);

                command.Parameters.AddWithValue(command, "@OrderType", model.OrderTypeId);

                command.Parameters.AddWithValue(command, "@OrderMainId", model.OrderMainId);

                command.Parameters.AddWithValue(command, "@ReferenceDocNo", model.ReferenceDocNo);

                command.Parameters.AddWithValue(command, "@InvoiceAmount", model.InvoiceAmount);

                command.Parameters.AddWithValue(command, "@CurrencyCode", model.CurrencyCode);

                command.Parameters.AddWithValue(command, "@LineDescription", model.LineDescription);

                command.Parameters.AddWithValue(command, "@VendorCode", model.VendorCode);

                command.Parameters.AddWithValue(command, "@DueDate", null);

                command.Parameters.AddWithValue(command, "@AgingDays", model.AgingDays);

                command.Parameters.AddWithValue(command, "@ProblematicInvoiceReasonId",
                    model.ProblematicInvoiceReasonId);

                command.Parameters.AddWithValue(command, "@ReasonAdditionalDescription", model.ReasonAdditionalDescription);
                command.Parameters.AddWithValue(command, "@Comment", model.Comment);

                command.Parameters.AddWithValue(command, "@AccountCode", model.AccountCode);
                command.Parameters.AddWithValue(command, "@WithHoldingTaxId", model.WithHoldingTaxId);
                command.Parameters.AddWithValue(command, "@TaxId", model.TaxId);
                command.Parameters.AddWithValue(command, "@TaxAmount", model.TaxAmount);
                command.Parameters.AddWithValue(command, "@GrossAmount", model.GrossAmount);
                command.Parameters.AddWithValue(command, "@OrderReference", model.OrderReference);
                command.Parameters.AddWithValue(command, "@InvoicePeriod", model.InvoicePeriod);
                command.Parameters.AddWithValue(command, "@VendorAccount",
                    model.VendorAccount);
                command.Parameters.AddWithValue(command, "@WithHoldingTaxAmount", model.WithHoldingTaxAmount);

				command.Parameters.AddWithValue(command, "@TransactionDate", model.TransactionDate);

				command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@NewInvoiceRegisterId", SqlDbType.Int);
                command.Parameters["@NewInvoiceRegisterId"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();
                int invoiceRegisterId = 0;
                if (reader.Read())
                {
                    invoiceRegisterId = reader.Get<int>("NewInvoiceRegisterId");
                }

                return invoiceRegisterId;
            }
        }

        public async Task<List<OrderListApproved>> GetOrderListApproved(int businessUnitId, string vendorCode)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"EXEC SP_OrderListApproved @BusinessUnitId, @VendorCode";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<OrderListApproved>();
            while (await reader.ReadAsync())
                list.Add(reader.GetByEntityStructure<OrderListApproved>());

            return list;
        }

        public async Task<List<ProblematicInvoiceReason>> GetProblematicInvoiceReasonList()
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"SELECT * FROM VW_ProblematicInvoiceReasonList";

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<ProblematicInvoiceReason>();
            while (await reader.ReadAsync())
                list.Add(reader.GetByEntityStructure<ProblematicInvoiceReason>());

            return list;
        }

        public async Task<bool> Delete(int item, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    @"EXEC SP_InvoiceRegister_IUD @InvoiceRegisterId,NULL,NULL,NULL,
                                                  NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                                                  NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                                                  NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                                                  NULL,NULL,NULL,NULL,@UserId,@NewInvoiceRegisterId = @NewInvoiceRegisterId
                    OUTPUT select @NewInvoiceRegisterId as NewInvoiceRegisterId";


                command.Parameters.AddWithValue(command, "@InvoiceRegisterId", item);

                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@NewInvoiceRegisterId", SqlDbType.Int);
                command.Parameters["@NewInvoiceRegisterId"].Direction = ParameterDirection.Output;

                await command.ExecuteNonQueryAsync();

                var returnValue = command.Parameters["@NewInvoiceRegisterId"].Value;
                return returnValue != DBNull.Value && returnValue != null ? true : false;
            }
        }

        public async Task<List<MatchingMainGRN>> MatchingMainGRN(InvoiceMatchingGRNModel model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_InvoiceMatchingMainGRN @businessUnitId,@dateFrom,@dateTo,@invoiceStatus,@toMatch";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@invoiceStatus", model.InvoiceStatus);
            command.Parameters.AddWithValue(command, "@toMatch", model.ToMatch);

            using var reader = await command.ExecuteReaderAsync();

            List<MatchingMainGRN> list = new List<MatchingMainGRN>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<MatchingMainGRN>());

            return list;
        }

        public async Task<List<MatchingMainService>> MatchingMainService(InvoiceMatchingGRNModel model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_InvoiceMatchingMainService @businessUnitId,@dateFrom,@dateTo,@invoiceStatus,@toMatch";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@invoiceStatus", model.InvoiceStatus);
            command.Parameters.AddWithValue(command, "@toMatch", model.ToMatch);

            using var reader = await command.ExecuteReaderAsync();

            List<MatchingMainService> list = new List<MatchingMainService>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<MatchingMainService>());

            return list;
        }

        public async Task<MatchingMain> GetMatchingMain(int orderMainId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterMatchingMain @OrderMainId";
            command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetByEntityStructure<MatchingMain>();

            return new MatchingMain();
        }

        public async Task<string> GetKeyKode(int orderMainId)
        {
            string result = null;
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"SELECT TOP 1
                                    ot.KeyCode
                                    FROM Procurement.OrderMain om1
                                    INNER JOIN Register.OrderTypes ot
                                    ON om1.OrderTypeId = ot.OrderTypeId
                                    WHERE om1.OrderMainId = @OrderMainId";
            command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
                result = reader.Get<string>("KeyCode");

            return result;
        }

        public async Task<List<InvoiceRegisterDetailForPO>> GetDetailsForPO(InvoiceGetDetailsModel model)
        {
            string grns = string.Join(',', model.GRNs);
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_InvoiceRegisterDetailsLoad @businessUnitId,@grns,@orderMainId,@date,@advanceAmount";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@grns", string.IsNullOrEmpty(grns) ? "-1" : grns);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);
            command.Parameters.AddWithValue(command, "@advanceAmount", model.AdvanceAmount);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceRegisterDetailForPO> list = new List<InvoiceRegisterDetailForPO>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterDetailForPO>());

            return list;
        }

        public async Task<List<InvoiceRegisterDetailForOther>> GetDetailsForOther(InvoiceGetDetailsModel model)
        {
            string grns = model.GRNs is null ? "-1" : string.Join(',', model.GRNs);
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_InvoiceRegisterDetailsLoad @businessUnitId,@grns,@orderMainId,@date,@advanceAmount";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@grns", string.IsNullOrEmpty(grns) ? "-1" : grns);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);
            command.Parameters.AddWithValue(command, "@advanceAmount", model.AdvanceAmount);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceRegisterDetailForOther> list = new List<InvoiceRegisterDetailForOther>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterDetailForOther>());

            return list;
        }

        public async Task<List<string>> GetTransactionReferenceList(int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_TransactionReferenceList @businessUnitId";
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<string>();
            while (await reader.ReadAsync())
                list.Add(reader.Get<string>("TREFERENCE"));

            return list;
        }

        public async Task<List<string>> GetReferenceList(int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_ReferenceList @businessUnitId";
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<string>();
            while (await reader.ReadAsync())
                list.Add(reader.Get<string>("GNRL_DESCR_25"));

            return list;
        }

        public async Task<List<string>> GetInvoiceList(int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceList @businessUnitId";
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<string>();
            while (await reader.ReadAsync())
                list.Add(reader.Get<string>("GNRL_DESCR_24"));

            return list;
        }

        public async Task<List<AdvanceInvoice>> GetAdvanceInvoicesList(int orderMainId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_AdvanceInvoicesList @OrderMainId";
            command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<AdvanceInvoice>();
            while (await reader.ReadAsync())
                list.Add(reader.GetByEntityStructure<AdvanceInvoice>());

            return list;
        }

        public async Task<int> SaveInvoiceMatchingMain(InvoiceMathcingMain request, int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;

            command.CommandText = @"EXEC SP_InvoiceMatchingMain_IUD 
                @InvoiceMatchingMainId,
                @BusinessUnitId,
                @OrderMainId,
                @InvoiceRegisterId,
                @WHT,
                @Comment,
                @TransactionDate,
                @TaxAmount,
                @GrossAmount,
                @OrderAmount,
                @SupplierWHTRate,
                @UserId,
                @Period,
                @NewInvoiceMatchingMainId = @NewInvoiceMatchingMainId OUTPUT select @NewInvoiceMatchingMainId 
                as NewInvoiceMatchingMainId";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", request.InvoiceMatchingMainId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", request.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@OrderMainId", request.OrderMainId);
            command.Parameters.AddWithValue(command, "@InvoiceRegisterId",
                request.InvoiceRegisterId == 0 ? null : request.InvoiceRegisterId);
            command.Parameters.AddWithValue(command, "@WHT", request.WHT);
            command.Parameters.AddWithValue(command, "@Comment", request.Comment);
            command.Parameters.AddWithValue(command, "@TransactionDate", request.TransactionDate);
            command.Parameters.AddWithValue(command, "@TaxAmount", request.TaxAmount);
            command.Parameters.AddWithValue(command, "@GrossAmount", request.GrossAmount);
            command.Parameters.AddWithValue(command, "@OrderAmount", request.OrderAmount);
            command.Parameters.AddWithValue(command, "@SupplierWHTRate", request.SupplierWHTRate);
            command.Parameters.AddWithValue(command, "@UserId", userId);
            command.Parameters.AddWithValue(command, "@Period", request.InvoicePeriod);
            command.Parameters.Add("@NewInvoiceMatchingMainId", SqlDbType.Int);
            command.Parameters["@NewInvoiceMatchingMainId"].Direction = ParameterDirection.Output;

            await using var reader = await command.ExecuteReaderAsync();
            int newInvoiceMatchingMainId = 0;
            if (reader.Read())
            {
                newInvoiceMatchingMainId = reader.Get<int>("NewInvoiceMatchingMainId");
            }

            return newInvoiceMatchingMainId;
        }

        public async Task<bool> SaveInvoiceMatchingGRNs(int requestInvoiceMatchingMainId, DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_InvoiceMatchingGRNs_IUD
                                    @InvoiceMatchingMainId,
                                    @RNEInvoicesMatchingType";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", requestInvoiceMatchingMainId);
            command.Parameters.AddTableValue(command, "@RNEInvoicesMatchingType", "RNEInvoicesMatchingType", dataTable);
            var value = await command.ExecuteNonQueryAsync();
            return value > 0;
        }

        public async Task<bool> SaveInvoiceMatchingAdvances(int requestMatchingId,
            DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_InvoiceMatchingAdvances_IUD
                                    @InvoiceMatchingMainId,
                                    @AdvanceInvoicesMatchingType";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", requestMatchingId);
            command.Parameters.AddTableValue(command, "@AdvanceInvoicesMatchingType", "AdvanceInvoicesMatchingType",
                dataTable);
            var value = await command.ExecuteNonQueryAsync();
            return value > 0;
        }

        public async Task<bool> SaveInvoiceMatchingDetails(int requestInvoiceMatchingMainid, DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_InvoiceMatchingDetails_IUD 
                                    @InvoiceMatchingMainid,
                                    @TVP";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainid", requestInvoiceMatchingMainid);
            command.Parameters.AddTableValue(command, "@TVP", "InvoicesMatchingDetailsType2",
                dataTable);
            var value = await command.ExecuteNonQueryAsync();
            return value > 0;
        }

        public async Task<bool> InvoiceIUD(int businessUnitId, int invoiceRegisterId, int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = _businessUnitHelper.BuildQueryForIntegration(businessUnitId,
                "SP_Invoice_IUD @BusinessUnitId, @InvoiceRegisterId, @UserId");
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@InvoiceRegisterId", invoiceRegisterId);
            command.Parameters.AddWithValue(command, "@UserId", userId);
            await _unitOfWork.SaveChangesAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<InvoiceRegisterByOrderMainId> InvoiceRegisterList(int orderMainId)
        {
            InvoiceRegisterByOrderMainId orders = new InvoiceRegisterByOrderMainId();
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;

            command.CommandText = @"EXEC SP_InvoiceRegisterOrderData
               @OrderMainId";

            command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

            await using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
                orders = reader.GetByEntityStructure<InvoiceRegisterByOrderMainId>();

            return orders;
        }

        public async Task<List<InvoiceRegisterServiceDetailsLoad>> InvoiceRegisterDetailsLoad(
            InvoiceRegisterServiceLoadModel model)
        {
            List<InvoiceRegisterServiceDetailsLoad> invoices = new List<InvoiceRegisterServiceDetailsLoad>();
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_InvoiceRegisterServiceDetailsLoad @businessUnitId,@orderMainId,@date,@totalAmount,@advanceAmount";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);
            command.Parameters.AddWithValue(command, "@totalAmount", model.TotalAmount);
            command.Parameters.AddWithValue(command, "@advanceAmount", model.AdvanceAmount);

            await using var reader = await command.ExecuteReaderAsync();

            var list = new List<InvoiceRegisterServiceDetailsLoad>();
            while (await reader.ReadAsync())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterServiceDetailsLoad>());

            return list;
        }

        public async Task<List<int>> GetDetailIds(int invoiceMatchingMainId)
        {
            List<int> invoices = new List<int>();
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"select InvoiceMatchingDetailId from Finance.InvoiceMatchingDetails where InvoiceMatchingMainid = @id";
            command.Parameters.AddWithValue(command, "@id", invoiceMatchingMainId);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                invoices.Add(reader.Get<int>("InvoiceMatchingDetailId"));

            return invoices;
        }

        public async Task<bool> CheckInvoiceRegister(int invoiceRegisterId, int businessUnit, string vendorCode,
            string invoiceNo)
        {
            int result = 0;
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"select InvoiceRegisterId from Finance.InvoiceRegister 
                                    where BusinessUnitId = @businessUnitId
                                    and   VendorCode = @vendorCode
                                    and   InvoiceNo = @invoiceNo";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnit);
            command.Parameters.AddWithValue(command, "@vendorCode", vendorCode);
            command.Parameters.AddWithValue(command, "@invoiceNo", invoiceNo);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                result = reader.Get<int>("InvoiceRegisterId");


            if (result == 0)
                return false;

            if (result == invoiceRegisterId)
                return false;

            return true;
        }

        public async Task<List<RegisterDraft>> RegisterDraft(InvoiceRegisterGetModel model, int userId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterDraft @businessUnitId,@dateFrom,@dateTo,@userId";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterDraft> list = new List<RegisterDraft>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterDraft>("Attachments"));

            return list;
        }

        public async Task<List<RegisterHeld>> RegisterHeld(InvoiceRegisterGetModel model, int userId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterHeld @businessUnitId,@dateFrom,@dateTo,@userId";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);
            command.Parameters.AddWithValue(command, "@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<RegisterHeld> list = new List<RegisterHeld>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<RegisterHeld>());

            return list;
        }

        public async Task<List<ApprovalInfo>> ApprovalInfos(int invoiceRegisterId, int userId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceApprovalInfo @registerId,@userId";
            command.Parameters.AddWithValue(command, "@registerId", invoiceRegisterId);
            command.Parameters.AddWithValue(command, "@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            List<ApprovalInfo> list = new List<ApprovalInfo>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<ApprovalInfo>());

            return list;
        }

        public async Task<List<InvoiceMatchingMainGRN>> MatchingMainGRNList(InvoiceMatchingMainModel model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingMainList @businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", model.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", model.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceMatchingMainGRN> list = new List<InvoiceMatchingMainGRN>();
            while (reader.Read())
                list.Add(reader.GetByEntityStructure<InvoiceMatchingMainGRN>());

            return list;
        }

        public async Task InvoiceIUDIntegration(int businessUnitId, int invoiceMatchingMainId, int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = _businessUnitHelper.BuildQueryForIntegration(businessUnitId,
                "SP_M_Invoice_IUD @BusinessUnitId, @InvoiceMatchingMainId, @UserId");

            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", invoiceMatchingMainId);
            command.Parameters.AddWithValue(command, "@UserId", userId);
            await _unitOfWork.SaveChangesAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<InvoiceMatchResultModel> GetInvoiceMatchData(int invoiceMatchingMainId, int businessUnitId)
        {
            InvoiceMatchResultModel resultModel = new InvoiceMatchResultModel();
            resultModel.InvoiceMatchMainData = await InvoiceMatchMainData(invoiceMatchingMainId);
            resultModel.InvoiceMatchDetailDatas = await InvoiceMatchDetailData(invoiceMatchingMainId, businessUnitId);
            resultModel.InvoiceMatchAdvances = await InvoiceMatchAdvance(invoiceMatchingMainId);
            resultModel.InvoiceMatchGRN = await InvoiceMatchGRN(invoiceMatchingMainId);
            return resultModel;

        }

        public async Task<InvoiceMatchMainData> InvoiceMatchMainData(int invoiceMatchMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingMainLoad @invoiceMatchingMainId";
            command.Parameters.AddWithValue(command, "@invoiceMatchingMainId", invoiceMatchMainId);

            using var reader = await command.ExecuteReaderAsync();

            InvoiceMatchMainData mainData = new InvoiceMatchMainData();
            if (reader.Read())
                mainData = reader.GetByEntityStructure<InvoiceMatchMainData>();

            return mainData;
        }

        public async Task<List<InvoiceMatchDetailData>> InvoiceMatchDetailData(int invoiceMatchMainId, int businessUnitId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingDetailsLoad @invoiceMatchingMainId,@businessUnitId";
            command.Parameters.AddWithValue(command, "@invoiceMatchingMainId", invoiceMatchMainId);
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceMatchDetailData> detailData = new List<InvoiceMatchDetailData>();
            while (reader.Read())
                detailData.Add(reader.GetByEntityStructure<InvoiceMatchDetailData>());

            return detailData;
        }

        public async Task<List<InvoiceMatchAdvance>> InvoiceMatchAdvance(int invoiceMatchMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingAdvancesLoad @invoiceMatchingMainId";
            command.Parameters.AddWithValue(command, "@invoiceMatchingMainId", invoiceMatchMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceMatchAdvance> advanceData = new List<InvoiceMatchAdvance>();
            while (reader.Read())
                advanceData.Add(reader.GetByEntityStructure<InvoiceMatchAdvance>());

            return advanceData;
        }

        public async Task<List<InvoiceMatchGRN>> InvoiceMatchGRN(int invoiceMatchMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingGRNLoad @invoiceMatchingMainId";
            command.Parameters.AddWithValue(command, "@invoiceMatchingMainId", invoiceMatchMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceMatchGRN> grnData = new List<InvoiceMatchGRN>();
            while (reader.Read())
                grnData.Add(reader.GetByEntityStructure<InvoiceMatchGRN>());

            return grnData;
        }

        public async Task<bool> InvoiceRegisterDetailsSave(int invoiceRegisterMainId, DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_InvoiceRegisterDetails_IUD
                                    @InvoiceRegisterMainId,
                                    @Data";

            command.Parameters.AddWithValue(command, "@InvoiceRegisterMainId", invoiceRegisterMainId);
            command.Parameters.AddTableValue(command, "@Data", "InvoiceRegisterDetailsType", dataTable);
            var value = await command.ExecuteNonQueryAsync();
            return value > 0;
        }


        public async Task<List<InvoiceRegisterPayablesTransactions>> GetInvoiceRegisterPayablesTransactions(int invoiceRegisterId)
        {
            List<InvoiceRegisterPayablesTransactions> list = new List<InvoiceRegisterPayablesTransactions>();
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec SP_InvoiceRegisterPayablesTransactions @invoiceRegisterId";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterPayablesTransactions>());

            return list;
        }

        public async Task<List<InvoiceRegisterGetDetails>> GetInvoiceRegisterDetailsLoad(int invoiceRegisterId)
        {
            List<InvoiceRegisterGetDetails> list = new List<InvoiceRegisterGetDetails>();
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterDetails @invoiceRegisterId";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterGetDetails>());

            return list;
        }

        public async Task<InvoiceRegisterLoad> GetInvoiceRegisterMainLoad(int invoiceRegisterId)
        {
            InvoiceRegisterLoad main = new InvoiceRegisterLoad();
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterMain @invoiceRegisterId";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

            using var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
                main = reader.GetByEntityStructure<InvoiceRegisterLoad>("InvoiceRegisterDetails",
                                                                        "WithHoldingTaxDatas", 
                                                                        "TaxDatas",
                                                                        "BusinessUnits");

            return main;
        }

        public async Task DeleteDetailsNotIncludes(List<int?> ids, int invoiceRegisterId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            string result = $"({string.Join(",", ids)})";
            command.CommandText = @"DELETE FROM Finance.InvoiceRegisterDetails WHERE InvoiceRegisterDetailId = @invoiceRegisterId
                                       AND InvoiceRegisterDetailId NOT IN " + result;
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAnalysisNotIncludes(List<int?> invoiceRegisterDetailIds)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            string result = $"({string.Join(",", invoiceRegisterDetailIds)})";
            command.CommandText = @"DELETE FROM Finance.InvoiceRegisterAnalysis WHERE  InvoiceRegisterDetailId NOT IN " + result;

            await command.ExecuteNonQueryAsync();
        }

		public async Task<InvoicePeriod> GetPeriod(int businessUnitId)
		{
			using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText = @"exec dbo.SP_PeriodListByBusinessId @BusinessUnitId";
			command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

			using var reader = await command.ExecuteReaderAsync();

			InvoicePeriod mainData = new InvoicePeriod();
			if (reader.Read())
				mainData = reader.GetByEntityStructure<InvoicePeriod>();

			return mainData;
		}

		public async Task<List<InvoiceRegisterOrderDetail>> GetRegisterOrderDetails(int orderMainId)
		{
			List<InvoiceRegisterOrderDetail> list = new List<InvoiceRegisterOrderDetail>();
			using var command = _unitOfWork.CreateCommand() as DbCommand;
			command.CommandText = @"exec dbo.SP_InvoiceRegisterOrderDetails @OrderMainId";
			command.Parameters.AddWithValue(command, "@OrderMainId", orderMainId);

			using var reader = await command.ExecuteReaderAsync();

			while (reader.Read())
				list.Add(reader.GetByEntityStructure<InvoiceRegisterOrderDetail>());

			return list;
		}
        
        public async Task<List<InvoiceRegisterAdvance>> GetInvoiceRegisterAdvance(int businessUnitId, DateTime dateFrom, DateTime dateTo, int userId)
        {
           var list = new List<InvoiceRegisterAdvance>();
           await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "exec SP_InvoiceRegisterAdvance @BusinessUnitId, @DateFrom, @DateTo, @UserId";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@DateFrom", dateFrom);
            command.Parameters.AddWithValue(command, "@DateTo", dateTo);
            command.Parameters.AddWithValue(command, "@UserId", userId);
            
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                list.Add(reader.GetByEntityStructure<InvoiceRegisterAdvance>());

            return list;
        }
	}
}
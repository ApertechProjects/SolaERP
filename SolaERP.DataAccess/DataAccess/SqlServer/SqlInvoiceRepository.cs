using MediatR;
using Microsoft.Win32;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Invoice;
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
            int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_InvoiceRegisterApprove @invoiceRegisterId,@sequence,@approveStatus,@userId,@comment";

                command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);
                command.Parameters.AddWithValue(command, "@sequence", sequence);
                command.Parameters.AddWithValue(command, "@approveStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@comment", comment);

                return await command.ExecuteNonQueryAsync() > 0;
            }
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
                                                  @InvoiceNo, @SystemInvoiceNo,@OrderType,@OrderMainId,
                    @ReferenceDocNo,@InvoiceAmount,@CurrencyCode, 
                    @LineDescription,@VendorCode,@DueDate,@AgingDays,
                    @ProblematicInvoiceReasonId,@Status,@ApproveStatus, @ReasonAdditionalDescription,@AccountCode
                    ,@UserId,@NewInvoiceRegisterId = @NewInvoiceRegisterId OUTPUT select @NewInvoiceRegisterId as NewInvoiceRegisterId";


                command.Parameters.AddWithValue(command, "@InvoiceRegisterId", model.InvoiceRegisterId);

                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);

                command.Parameters.AddWithValue(command, "@InvoiceType", model.InvoiceType);

                command.Parameters.AddWithValue(command, "@InvoiceDate", model.InvoiceDate);

                command.Parameters.AddWithValue(command, "@InvoiceReceivedDate", model.InvoiceReceivedDate);

                command.Parameters.AddWithValue(command, "@InvoiceNo", model.InvoiceNo);

                command.Parameters.AddWithValue(command, "@SystemInvoiceNo", model.SystemInvoiceNo);

                command.Parameters.AddWithValue(command, "@OrderType", model.OrderType);

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

                command.Parameters.AddWithValue(command, "@Status", model.Status);

                command.Parameters.AddWithValue(command, "@ApproveStatus", model.ApproveStatus);

                command.Parameters.AddWithValue(command, "@ReasonAdditionalDescription", model.ReasonAdditionalDescription);

                command.Parameters.AddWithValue(command, "@AccountCode",
                    model.AccountCode);

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
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    "EXEC SP_InvoiceRegister_IUD @InvoiceRegisterId,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,@UserId,NULL";


                command.Parameters.AddWithValue(command, "@InvoiceRegisterId", item);

                command.Parameters.AddWithValue(command, "@UserId", userId);

                await _unitOfWork.SaveChangesAsync();

                return await command.ExecuteNonQueryAsync() > 0;
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
            command.CommandText = @"exec dbo.SP_InvoiceRegisterDetailsLoad @businessUnitId,@grns,@orderMainId,@date";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@grns", string.IsNullOrEmpty(grns) ? "-1" : grns);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);

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
            command.CommandText = @"exec dbo.SP_InvoiceRegisterDetailsLoad @businessUnitId,@grns,@orderMainId,@date";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@grns", string.IsNullOrEmpty(grns) ? "-1" : grns);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);

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
                @UserId,
                @NewInvoiceMatchingMainId = @NewInvoiceMatchingMainId OUTPUT select @NewInvoiceMatchingMainId 
                as NewInvoiceMatchingMainId";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", request.InvoiceMatchingMainId);
            command.Parameters.AddWithValue(command, "@BusinessUnitId", request.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@OrderMainId", request.OrderMainId);
            command.Parameters.AddWithValue(command, "@InvoiceRegisterId", request.InvoiceRegisterId);
            command.Parameters.AddWithValue(command, "@WHT", request.WHT);
            command.Parameters.AddWithValue(command, "@Comment", request.Comment);
            command.Parameters.AddWithValue(command, "@TransactionDate", request.TransactionDate);
            command.Parameters.AddWithValue(command, "@UserId", userId);
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

        public async Task<bool> SaveInvoiceMatchingAdvances(int requestInvoiceRegisterId, int requestMatchingId,
            DataTable dataTable)
        {
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = @"EXEC SP_InvoiceMatchingAdvances_IUD
                                    @InvoiceMatchingMainId,
                                    @InvoiceRegisterId,
                                    @AdvanceInvoicesMatchingType";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainId", requestMatchingId);
            command.Parameters.AddWithValue(command, "@InvoiceRegisterId", requestInvoiceRegisterId);
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
                                    @InvoicesMatchingDetailsType";

            command.Parameters.AddWithValue(command, "@InvoiceMatchingMainid", requestInvoiceMatchingMainid);
            command.Parameters.AddTableValue(command, "@InvoicesMatchingDetailsType", "InvoicesMatchingDetailsType",
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

        public async Task<List<InvoiceRegisterServiceDetailsLoad>> InvoiceRegisterDetailsLoad(InvoiceRegisterLoadModel model)
        {
            List<InvoiceRegisterServiceDetailsLoad> invoices = new List<InvoiceRegisterServiceDetailsLoad>();
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterServiceDetailsLoad @businessUnitId,@orderMainId,@date,@totalAmount";
            command.Parameters.AddWithValue(command, "@businessUnitId", model.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@orderMainId", model.OrderMainId);
            command.Parameters.AddWithValue(command, "@date", model.Date);
            command.Parameters.AddWithValue(command, "@totalAmount", model.TotalAmount); 

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
            command.CommandText = @"select InvoiceMatchingDetailId from Finance.InvoiceMatchingDetails where InvoiceMatchingMainid = @id";
            command.Parameters.AddWithValue(command, "@id", invoiceMatchingMainId);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                invoices.Add(reader.Get<int>("InvoiceMatchingDetailId"));

            return invoices;
        }
    }
}
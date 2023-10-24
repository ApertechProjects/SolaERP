using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Reflection;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlInvoiceRepository : IInvoiceRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlInvoiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<List<RegisterLoadGRN>> RegisterLoadGRN(int invoiceRegisterId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceRegisterLoadGRNs @invoiceRegisterId";
            command.Parameters.AddWithValue(command, "@invoiceRegisterId", invoiceRegisterId);

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

        public async Task<bool> Save(InvoiceRegisterSaveModel model, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    "EXEC SP_InvoiceRegister_IUD @InvoiceRegisterId,@BusinessUnitId,@InvoiceType,@InvoiceDate,@InvoiceReceivedDate,@InvoiceNo,@SystemInvoiceNo,@OrderType,@OrderMainId,@ReferenceDocNo,@InvoiceAmount,@CurrencyCode,@LineDescription,@VendorCode,@DueDate,@AgingDays,@ProblematicInvoiceReasonId,@Status,@ApproveStatus,@ReasonAdditionalDescription,@UserId";


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

                command.Parameters.AddWithValue(command, "@ReasonAdditionalDescription",
                    model.ReasonAdditionalDescription);

                command.Parameters.AddWithValue(command, "@UserId", userId);

                await _unitOfWork.SaveChangesAsync();

                return await command.ExecuteNonQueryAsync() > 0;
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
                    "EXEC SP_InvoiceRegister_IUD @InvoiceRegisterId,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,@UserId";


                command.Parameters.AddWithValue(command, "@InvoiceRegisterId", item);

                command.Parameters.AddWithValue(command, "@UserId", userId);

                await _unitOfWork.SaveChangesAsync();

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<MatchingMainGRN>> MatchingMainGRN(InvoiceMatchingGRNModel model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_InvoiceMatchingMainGRN @businessUnitId,@dateFrom,@dateTo,@invoiceStatus,@toMatch";
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
            command.CommandText = @"exec dbo.SP_InvoiceMatchingMainService @businessUnitId,@dateFrom,@dateTo,@invoiceStatus,@toMatch";
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
    }
}
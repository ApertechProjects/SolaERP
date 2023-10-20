using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlInvoiceRepository : IInvoiceRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceService _invoiceService;
        public SqlInvoiceRepository(IInvoiceService invoiceService, IUnitOfWork unitOfWork)
        {
            _invoiceService = invoiceService;
            _unitOfWork = unitOfWork;
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
    }
}

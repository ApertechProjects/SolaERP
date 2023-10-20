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

using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlPaymentRepository : IPaymentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlPaymentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CreateAdvance>> CreateAdvanceAsync(CreateAdvanceModel createBalance)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentCreateAdvance @vendorCode,@currencyCode,@businessUnitId";
            command.Parameters.AddWithValue(command, "@vendorCode", createBalance.VendorCode);
            command.Parameters.AddWithValue(command, "@currencyCode", createBalance.CurrencyCode);
            command.Parameters.AddWithValue(command, "@businessUnitId", createBalance.BusinessUnitId);

            using var reader = await command.ExecuteReaderAsync();

            List<CreateAdvance> createAdvances = new List<CreateAdvance>();
            while (reader.Read())
                createAdvances.Add(GetCreateAdvance(reader));

            return createAdvances;
        }

        public async Task<List<CreateBalance>> CreateBalanceAsync(CreateBalanceModel createBalance)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentCreateBalance @vendorCode,@businessUnitId,@type";
            command.Parameters.AddWithValue(command, "@vendorCode", createBalance.VendorCode);
            command.Parameters.AddWithValue(command, "@businessUnitId", createBalance.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@type", createBalance.Type);

            using var reader = await command.ExecuteReaderAsync();

            List<CreateBalance> createBalances = new List<CreateBalance>();
            while (reader.Read())
                createBalances.Add(GetCreateBalance(reader));

            return createBalances;
        }

        public async Task<List<CreateOrder>> CreateOrderAsync(CreateOrderModel createOrder)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentCreateOrders @vendorCode,@businessUnitId,@currencyCode";
            command.Parameters.AddWithValue(command, "@vendorCode", createOrder.VendorCode);
            command.Parameters.AddWithValue(command, "@businessUnitId", createOrder.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@currencyCode", createOrder.CurrencyCode);

            using var reader = await command.ExecuteReaderAsync();

            List<CreateOrder> createOrders = new List<CreateOrder>();
            while (reader.Read())
                createOrders.Add(GetCreateOrder(reader));

            return createOrders;
        }

        private CreateBalance GetCreateBalance(DbDataReader reader)
        {
            return new CreateBalance
            {
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                Amount = reader.Get<decimal>("Amount")
            };
        }

        private CreateAdvance GetCreateAdvance(DbDataReader reader)
        {
            return new CreateAdvance
            {
                AccountCode = reader.Get<string>("AccountCode"),
                AccountName = reader.Get<string>("AccountName"),
                AmountToPay = reader.Get<decimal>("AmountToPay"),
                Budget = reader.Get<string>("Budget"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                InvoiceNo = reader.Get<string>("InvoiceNo"),
                OrderNo = reader.Get<string>("OrderNo"),
                OrderTotal = reader.Get<decimal>("OrderTotal"),
                PayableAmount = reader.Get<decimal>("PayableAmount"),
                PaymentRequestAmount = reader.Get<decimal>("PaymentRequestAmount"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                PaymentTermsName = reader.Get<string>("PaymentTermsName"),
                SystemInvoiceNo = reader.Get<string>("SystemInvoiceNo")
            };
        }

        private CreateOrder GetCreateOrder(DbDataReader reader)
        {
            return new CreateOrder
            {
                AccountCode = reader.Get<string>("AccountCode"),
                AccountName = reader.Get<string>("AccountName"),
                AmountToPay = reader.Get<decimal>("AmountToPay"),
                Budget = reader.Get<string>("Budget"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                InvoiceNo = reader.Get<string>("InvoiceNo"),
                Employee = reader.Get<string>("Employee"),
                OrderTotal = reader.Get<decimal>("OrderTotal"),
                PayableAmount = reader.Get<decimal>("PayableAmount"),
                GRNAmount = reader.Get<decimal>("GRNAmount"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                LinkAccount = reader.Get<string>("LinkAccount"),
                SystemInvoiceNo = reader.Get<string>("SystemInvoiceNo"),
                PaymentRequestAmount = reader.Get<decimal>("PaymentRequestAmount"),
                Reference = reader.Get<string>("Reference"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                WellNo = reader.Get<string>("WellNo")
            };
        }

        private InfoDetail GetInfoDetail(DbDataReader reader)
        {
            return new InfoDetail
            {
                AccountCode = reader.Get<string>("AccountCode"),
                AccountName = reader.Get<string>("AccountName"),
                AmountToPay = reader.Get<decimal>("AmountToPay"),
                Budget = reader.Get<string>("Budget"),
                PaymenRequestAmount = reader.Get<decimal>("PaymenRequestAmount"),
                InvoiceNo = reader.Get<string>("InvoiceNo"),
                Employee = reader.Get<string>("Employee"),
                PayableAmount = reader.Get<decimal>("PayableAmount"),
                GRNAmount = reader.Get<decimal>("GRNAmount"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                LinkAccount = reader.Get<string>("LinkAccount"),
                SystemInvoiceNo = reader.Get<string>("SystemInvoiceNo"),
                Reference = reader.Get<string>("Reference"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                WellNo = reader.Get<string>("WellNo"),
                PaymentDocumentDetailId = reader.Get<int>("PaymentDocumentDetailId"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId")
            };
        }

        private InfoApproval GetInfoApproval(DbDataReader reader)
        {
            return new InfoApproval
            {
                ApproveDate = reader.Get<DateTime>("ApproveDate"),
                ApproveStatus = reader.Get<int>("ApproveStatus"),
                Comment = reader.Get<string>("Comment"),
                Description = reader.Get<string>("Description"),
                FullName = reader.Get<string>("FullName"),
                Sequence = reader.Get<int>("Sequence"),
                SignaturePhoto = reader.Get<string>("SignaturePhoto")
            };
        }

        public async Task<InfoHeader> InfoHeader(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentMainLoad @paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

            using var reader = await command.ExecuteReaderAsync();

            InfoHeader infoHeader = new InfoHeader();
            if (reader.Read())
                infoHeader = reader.GetByEntityStructure<InfoHeader>();

            return infoHeader;
        }

        public async Task<List<InfoDetail>> InfoDetail(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentDetailsLoad @paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<InfoDetail> infoDetail = new List<InfoDetail>();
            while (reader.Read())
                infoDetail.Add(GetInfoDetail(reader));

            return infoDetail;
        }

        public async Task<List<InfoApproval>> InfoApproval(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentApprovalInformation @paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<InfoApproval> infoApproval = new List<InfoApproval>();
            while (reader.Read())
                infoApproval.Add(GetInfoApproval(reader));

            return infoApproval;
        }
    }
}

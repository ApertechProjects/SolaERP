using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;

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

        public async Task<List<All>> All(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentAll @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<All> list = new List<All>();
            while (reader.Read())
                list.Add(GetAll(reader));

            return list;
        }

        private WaitingForApproval GetWaitingForApproval(DbDataReader reader)
        {
            return new WaitingForApproval
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                Sequence = reader.Get<int>("Sequence")
            };
        }

        private Draft GetDraft(DbDataReader reader)
        {
            return new Draft
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName")
            };
        }

        private All GetAll(DbDataReader reader)
        {
            return new All
            {
                Amount = reader.Get<decimal>("Amount"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                PaymentStatus = reader.Get<string>("PaymentStatus"),
                Status = reader.Get<string>("Status"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName")
            };
        }

        private Approved GetApproved(DbDataReader reader)
        {
            return new Approved
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
            };
        }

        private Rejected GetRejected(DbDataReader reader)
        {
            return new Rejected
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                RejectComment = reader.Get<string>("RejectComment")
            };
        }

        private Held GetHeld(DbDataReader reader)
        {
            return new Held
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                HoldComment = reader.Get<string>("HoldComment")
            };
        }

        private Bank GetBank(DbDataReader reader)
        {
            return new Bank
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentDocumentPriorityId = reader.Get<int>("PaymentDocumentPriorityId"),
                PaymentDocumentTypeId = reader.Get<int>("PaymentDocumentTypeId"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName")
            };
        }

        public async Task<List<Approved>> Approved(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentApproved @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<Approved> list = new List<Approved>();
            while (reader.Read())
                list.Add(GetApproved(reader));

            return list;
        }

        public async Task<List<Bank>> Bank(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentBank @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<Bank> list = new List<Bank>();
            while (reader.Read())
                list.Add(GetBank(reader));

            return list;
        }

        public async Task<List<Draft>> Draft(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentDrafts @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<Draft> list = new List<Draft>();
            while (reader.Read())
                list.Add(GetDraft(reader));

            return list;
        }

        public async Task<List<Held>> Held(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentHeld @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<Held> list = new List<Held>();
            while (reader.Read())
                list.Add(GetHeld(reader));

            return list;
        }

        public async Task<List<Rejected>> Rejected(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentRejected @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<Rejected> list = new List<Rejected>();
            while (reader.Read())
                list.Add(GetRejected(reader));

            return list;
        }

        public async Task<List<WaitingForApproval>> WaitingForApproval(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentWFA @userId,@businessUnitId,@dateFrom,@dateTo";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);

            using var reader = await command.ExecuteReaderAsync();

            List<WaitingForApproval> list = new List<WaitingForApproval>();
            while (reader.Read())
                list.Add(GetWaitingForApproval(reader));

            return list;
        }

        public async Task<bool> SendToApprove(int userId, int paymentDocumentMainId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_PaymentDocumentSendToApprove @userId,@paymentDocumentMainId";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}

using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.PortableExecutable;

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
            string vendorCode = string.Join(',', createBalance.VendorCode);
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentCreateBalance @vendorCode,@businessUnitId,@type";
            command.Parameters.AddWithValue(command, "@vendorCode",
                string.IsNullOrEmpty(vendorCode) ? "-1" : vendorCode);
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
                TransactionReference = reader.Get<string>("TransactionReference"),
                OrderTotal = reader.Get<decimal>("OrderTotal"),
                PayableAmount = reader.Get<decimal>("PayableAmount"),
                PaymentRequestAmount = reader.Get<decimal>("PaymentRequestAmount"),
                PaymentTerms = reader.Get<string>("PaymentTerms"),
                PaymentTermsName = reader.Get<string>("PaymentTermsName"),
                SystemInvoiceNo = reader.Get<string>("SystemInvoiceNo"),
                Department = reader.Get<string>("Department")
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
                WellNo = reader.Get<string>("WellNo"),
                PaymentTermsName = reader.Get<string>("PaymenttermsName"),
                AgingDays = reader.Get<int>("AgingDays")
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
                PaymentRequestAmount = reader.Get<decimal>("PaymentRequestAmount"),
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
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                PaymentTermName = reader.Get<string>("PaymentTermName"),
                AgingDays = reader.Get<int>("AgingDays")
            };
        }

        private InfoApproval GetInfoApproval(DbDataReader reader)
        {
            return new InfoApproval
            {
                ApproveDate = reader.Get<DateTime>("ApproveDate"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
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
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentAll @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

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
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                Sequence = reader.Get<int>("Sequence"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        private Draft GetDraft(DbDataReader reader)
        {
            return new Draft
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        private All GetAll(DbDataReader reader)
        {
            return new All
            {
                Amount = reader.Get<decimal>("Amount"),
                ApproveStatus = reader.Get<string>("ApproveStatus"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                Comment = reader.Get<string>("Comment"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                PaymentStatus = reader.Get<string>("PaymentStatus"),
                Status = reader.Get<string>("Status"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment"),
                RejectHoldComment = reader.Get<string>("RejectHoldComment")
            };
        }

        private Approved GetApproved(DbDataReader reader)
        {
            return new Approved
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        private Rejected GetRejected(DbDataReader reader)
        {
            return new Rejected
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                RejectComment = reader.Get<string>("RejectComment"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        private Held GetHeld(DbDataReader reader)
        {
            return new Held
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                HoldComment = reader.Get<string>("HoldComment"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        private Bank GetBank(DbDataReader reader)
        {
            return new Bank
            {
                Amount = reader.Get<decimal>("Amount"),
                Comment = reader.Get<string>("Comment"),
                PaymentType = reader.Get<string>("OrderAdvance"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                PaymentDocumentMainId = reader.Get<int>("PaymentDocumentMainId"),
                Priority = reader.Get<string>("Priority"),
                PaymentRequestDate = reader.Get<DateTime>("PaymentRequestDate"),
                PaymentRequestNo = reader.Get<string>("PaymentRequestNo"),
                TransactionReference = reader.Get<string>("TransactionReference"),
                VendorCode = reader.Get<string>("VendorCode"),
                VendorName = reader.Get<string>("VendorName"),
                SentDate = reader.Get<DateTime?>("SentDate"),
                LineNo = reader.Get<long>("LineNo"),
                HasAttachment = reader.Get<bool>("HasAttachment")
            };
        }

        public async Task<List<Approved>> Approved(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentApproved @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

            using var reader = await command.ExecuteReaderAsync();

            List<Approved> list = new List<Approved>();
            while (reader.Read())
                list.Add(GetApproved(reader));

            return list;
        }

        public async Task<List<Bank>> Bank(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentBank @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

            using var reader = await command.ExecuteReaderAsync();

            List<Bank> list = new List<Bank>();
            while (reader.Read())
                list.Add(GetBank(reader));

            return list;
        }

        public async Task<List<Draft>> Draft(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentDrafts @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

            using var reader = await command.ExecuteReaderAsync();

            List<Draft> list = new List<Draft>();
            while (reader.Read())
                list.Add(GetDraft(reader));

            return list;
        }

        public async Task<List<Held>> Held(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentHeld @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");


            using var reader = await command.ExecuteReaderAsync();

            List<Held> list = new List<Held>();
            while (reader.Read())
                list.Add(GetHeld(reader));

            return list;
        }

        public async Task<List<Rejected>> Rejected(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentRejected @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

            using var reader = await command.ExecuteReaderAsync();

            List<Rejected> list = new List<Rejected>();
            while (reader.Read())
                list.Add(GetRejected(reader));

            return list;
        }

        public async Task<List<WaitingForApproval>> WaitingForApproval(int userId, PaymentGetModel payment)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentDocumentWFA @userId,@businessUnitId,@dateFrom,@dateTo,@vendorCode";
            command.Parameters.AddWithValue(command, "@userId", userId);
            command.Parameters.AddWithValue(command, "@businessUnitId", payment.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@dateFrom", payment.DateFrom);
            command.Parameters.AddWithValue(command, "@dateTo", payment.DateTo);
            command.Parameters.AddWithValue(command, "@vendorCode", payment.VendorCode ?? "-1");

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
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_PaymentDocumentSendToApprove @userId,@paymentDocumentMainId";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<PaymentDocumentSaveResultModel> MainSave(int userId, PaymentDocumentMainSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_PaymentDocumentMain_IUD 
                                                                @PaymentDocumentMainId,
                                                                @BusinessUnitId,
                                                                @VendorCode,
                                                                @CurrencyCode,
                                                                @Comment,
                                                                @OrderAdvance,
                                                                @PaymentDocumentTypeId,
                                                                @PaymentDocumentPriorityId,
                                                                @ApproveStageMainId,
                                                                @PaymentRequestNo,
                                                                @PaymentRequestDate,
                                                                @SentDate,
                                                                @UserId,
                                                                @NewPaymentDocumentMainId = @NewPaymentDocumentMainId OUTPUT,
                                                                @NewPaymentRequestNo = @NewPaymentRequestNo OUTPUT 
                                                                select @NewPaymentDocumentMainId as NewPaymentDocumentMainId,
                                                               
                                                                @NewPaymentRequestNo as NewPaymentRequestNo";

                command.Parameters.AddWithValue(command, "@PaymentDocumentMainId", model.PaymentDocumentMainId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@VendorCode", model.VendorCode);
                command.Parameters.AddWithValue(command, "@CurrencyCode", model.CurrencyCode);
                command.Parameters.AddWithValue(command, "@Comment", model.Comment);
                command.Parameters.AddWithValue(command, "@OrderAdvance", model.OrderAdvance);
                command.Parameters.AddWithValue(command, "@PaymentDocumentTypeId", model.PaymentAttachmentTypeId);
                command.Parameters.AddWithValue(command, "@PaymentDocumentPriorityId", model.PaymentDocumentPriorityId);
                command.Parameters.AddWithValue(command, "@ApproveStageMainId", model.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@PaymentRequestNo", model.PaymentRequestNo);
                command.Parameters.AddWithValue(command, "@PaymentRequestDate", model.PaymentRequestDate);
                command.Parameters.AddWithValue(command, "@SentDate", null);
                command.Parameters.AddWithValue(command, "@UserId", userId);


                command.Parameters.Add("@NewPaymentDocumentMainId", SqlDbType.Int);
                command.Parameters["@NewPaymentDocumentMainId"].Direction = ParameterDirection.Output;

                command.Parameters.Add("@NewPaymentRequestNo", SqlDbType.NVarChar, 50);
                command.Parameters["@NewPaymentRequestNo"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();
                int requestId = 0;
                string requestNo = "";
                if (reader.Read())
                {
                    requestId = reader.Get<int>("NewPaymentDocumentMainId");
                    requestNo = reader.Get<string>("NewPaymentRequestNo");
                }

                return new PaymentDocumentSaveResultModel
                { PaymentDocumentMainId = requestId, PaymentRequestNo = requestNo };
            }
        }

        public async Task<bool> DetailSave(DataTable model)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_PaymentDocumentDetails_IUD @Data";
                command.Parameters.AddTableValue(command, "@Data", "PaymentDocumentDetailsType", model);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> Delete(int paymentDocumentMainId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    "EXEC SP_PaymentDocumentMain_IUD \r\n                                                               @PaymentDocumentMainId,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,@UserId,@NewPaymentDocumentMainId = @NewPaymentDocumentMainId OUTPUT,@NewPaymentRequestNo = @NewPaymentRequestNo OUTPUT select @NewPaymentDocumentMainId as NewPaymentDocumentMainId, @NewPaymentRequestNo as NewPaymentRequestNo";
                command.Parameters.AddWithValue(command, "@PaymentDocumentMainId", paymentDocumentMainId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@NewPaymentDocumentMainId", SqlDbType.Int);
                command.Parameters["@NewPaymentDocumentMainId"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@NewPaymentRequestNo", SqlDbType.NVarChar, 20);
                command.Parameters["@NewPaymentRequestNo"].Direction = ParameterDirection.Output;
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<decimal> VendorBalance(int businessUnitId, string vendorCode)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"EXEC SP_PaymentDocumentVendorBalance 
                                                                @VendorCode,
                                                                @BusinessUnitId"
                    ;

                command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();
                decimal result = 0;
                if (reader.Read())
                {
                    result = reader.Get<decimal>("Amount");
                }

                return result;
            }
        }

        public async Task<List<AttachmentDto>> InfoAttachments(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentAttachmentsDataLoad @paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);

            using var reader = await command.ExecuteReaderAsync();

            List<AttachmentDto> list = new List<AttachmentDto>();
            while (reader.Read())
                list.Add(GetAttachmentSubTypes(reader));

            return list;
        }

        public AttachmentDto GetAttachmentSubTypes(DbDataReader reader)
        {
            return new AttachmentDto
            {
                Checked = reader.Get<bool>("Checked"),
                AttachmentSubType = reader.Get<string>("PaymentDocumentSubType"),
                AttachmentSubTypeId = reader.Get<int>("PaymentDocumentSubTypeId"),
                AttachmentType = reader.Get<string>("PaymentDocumentType"),
                AttachmentTypeId = reader.Get<int>("PaymentDocumentTypeId")
            };
        }

        public AttachmentTypes GetAttachmentTypes(DbDataReader reader)
        {
            return new AttachmentTypes
            {
                TypeId = reader.Get<int>("PaymentDocumentTypeId"),
                TypeName = reader.Get<string>("PaymentDocumentType")
            };
        }

        public async Task<bool> ChangeStatus(int userId, int paymentDocumentId, int sequence, int approveStatus,
            string comment)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_PaymentDocumentApprove @UserId,@PaymentDocumentMainId,@Sequence,@ApproveStatus,@Comment";
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@PaymentDocumentMainId", paymentDocumentId);
                command.Parameters.AddWithValue(command, "@Sequence", sequence);
                command.Parameters.AddWithValue(command, "@ApproveStatus", approveStatus);
                command.Parameters.AddWithValue(command, "@Comment", comment);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<List<InvoiceLink>> InvoiceLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.InvoiceLink);

            using var reader = await command.ExecuteReaderAsync();

            List<InvoiceLink> invoiceLinks = new List<InvoiceLink>();
            while (reader.Read())
                invoiceLinks.Add(GetInvoiceLink(reader));

            return invoiceLinks;
        }

        public InvoiceLink GetInvoiceLink(DbDataReader reader)
        {
            return new InvoiceLink
            {
                InvoiceId = reader.Get<int>("InvoiceId"),
                InvoiceNo = reader.Get<string>("InvoiceNo")
            };
        }

        public async Task<List<OrderLink>> OrderLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.OrderLink);

            using var reader = await command.ExecuteReaderAsync();

            List<OrderLink> orderLinks = new List<OrderLink>();
            while (reader.Read())
                orderLinks.Add(GetOrderLink(reader));

            return orderLinks;
        }

        public OrderLink GetOrderLink(DbDataReader reader)
        {
            return new OrderLink
            {
                OrderMainId = reader.Get<int>("OrderMainId"),
                OrderNo = reader.Get<string>("OrderNo")
            };
        }

        public async Task<List<BidComparisonLink>> BidComparisonLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.BidComparisonLink);

            using var reader = await command.ExecuteReaderAsync();

            List<BidComparisonLink> bidComparisonLinks = new List<BidComparisonLink>();
            while (reader.Read())
                bidComparisonLinks.Add(GetBidComparisonLink(reader));

            return bidComparisonLinks;
        }

        public BidComparisonLink GetBidComparisonLink(DbDataReader reader)
        {
            return new BidComparisonLink
            {
                BidComparisonId = reader.Get<int>("OrderMainId"),
                ComparisonNo = reader.Get<string>("OrderNo")
            };
        }

        public async Task<List<BidLink>> BidLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.BidLink);

            using var reader = await command.ExecuteReaderAsync();

            List<BidLink> bidLinks = new List<BidLink>();
            while (reader.Read())
                bidLinks.Add(GetBidLink(reader));

            return bidLinks;
        }

        public BidLink GetBidLink(DbDataReader reader)
        {
            return new BidLink
            {
                BidMainId = reader.Get<int>("BidMainId"),
                BidNo = reader.Get<string>("BidNo")
            };
        }

        public async Task<List<RFQLink>> RFQLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.RFQLink);

            using var reader = await command.ExecuteReaderAsync();
            List<RFQLink> rfqLinks = new List<RFQLink>();
            while (reader.Read())
                rfqLinks.Add(GetRFQLink(reader));

            return rfqLinks;
        }

        public RFQLink GetRFQLink(DbDataReader reader)
        {
            return new RFQLink
            {
                RFQMainId = reader.Get<int>("RFQMainId"),
                RFQNo = reader.Get<string>("RFQNo")
            };
        }

        public async Task<List<RequestLink>> RequestLinks(int paymentDocumentMainId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec [dbo].[SP_PaymentDocumentsOtherAttachments] @paymentDocumentMainId,@attachmentType";
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainId);
            command.Parameters.AddWithValue(command, "@attachmentType", AttachmentPaymentTypes.RequestLink);

            using var reader = await command.ExecuteReaderAsync();

            List<RequestLink> rfqLinks = new List<RequestLink>();
            while (reader.Read())
                rfqLinks.Add(GetRequestLink(reader));

            return rfqLinks;
        }

        public RequestLink GetRequestLink(DbDataReader reader)
        {
            return new RequestLink
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                RequestNo = reader.Get<string>("RequestNo")
            };
        }

        public async Task<List<AttachmentTypes>> AttachmentTypes()
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"select * from Register.PaymentDocumentTypes";

            using var reader = await command.ExecuteReaderAsync();

            List<AttachmentTypes> list = new List<AttachmentTypes>();
            while (reader.Read())
                list.Add(GetAttachmentTypes(reader));

            return list;
        }

        public async Task<List<PaymentRequest>> PaymentRequest(PaymentRequestGetModel model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentDocumentCreateDocuments @vendorCode,@currencyCode";
            command.Parameters.AddWithValue(command, "@vendorCode", model.VendorCode);
            command.Parameters.AddWithValue(command, "@currencyCode", model.CurrencyCode);

            using var reader = await command.ExecuteReaderAsync();

            List<PaymentRequest> list = new List<PaymentRequest>();
            while (await reader.ReadAsync()) list.Add(reader.GetByEntityStructure<PaymentRequest>());

            return list;
        }

        public async Task<bool> PaymentOperation(int userId, DataTable table, PaymentOperations operation)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_PaymentDocumentChangeStatus @UserId,@PaymentDocumentMainId,@ApproveStatus";
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddTableValue(command, "@PaymentDocumentMainId", "SingleIdItems", table);
                command.Parameters.AddWithValue(command, "@ApproveStatus", operation);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<PaymentOrderMain> PaymentOrderMainLoad(PaymentOrderParamModel model)
        {
            string paymentDocumentMainIds = string.Join(",", model.PaymentDocumentMainIds);
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentOrderMainLoad @paymentOrderMainId,@paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentOrderMainId", model.PaymentOrderMainId);
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainIds);

            using var reader = await command.ExecuteReaderAsync();

            PaymentOrderMain data = new PaymentOrderMain();
            while (await reader.ReadAsync()) data = reader.GetByEntityStructure<PaymentOrderMain>();

            return data;
        }

        public async Task<List<PaymentOrderDetail>> PaymentOrderDetailLoad(PaymentOrderParamModel model)
        {
            string paymentDocumentMainIds = string.Join(",", model.PaymentDocumentMainIds);
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_PaymentOrderDetailsLoad @paymentOrderMainId,@paymentDocumentMainId";
            command.Parameters.AddWithValue(command, "@paymentOrderMainId", model.PaymentOrderMainId);
            command.Parameters.AddWithValue(command, "@paymentDocumentMainId", paymentDocumentMainIds);

            using var reader = await command.ExecuteReaderAsync();

            List<PaymentOrderDetail> datas = new List<PaymentOrderDetail>();
            while (await reader.ReadAsync()) datas.Add(reader.GetByEntityStructure<PaymentOrderDetail>());

            return datas;
        }

        public async Task<List<PaymentOrderTransaction>> PaymentOrderTransaction(DataTable table,
            int paymentOrderMainId, DateTime paymentDate, string bankAccount, decimal bankCharge)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"exec dbo.SP_PaymentOrderDetailsTransactions @paymentOrderMainId,@paymentDocuments,@paymentDate,@bankAccount,@bankCharge";
            command.Parameters.AddWithValue(command, "@paymentOrderMainId", paymentOrderMainId);
            command.Parameters.AddTableValue(command, "@paymentDocuments", "PaymentDocumentPay", table);
            command.Parameters.AddWithValue(command, "@paymentDate", paymentDate);
            command.Parameters.AddWithValue(command, "@bankAccount", bankAccount);
            command.Parameters.AddWithValue(command, "@bankCharge", bankCharge);

            using var reader = await command.ExecuteReaderAsync();

            List<PaymentOrderTransaction> datas = new List<PaymentOrderTransaction>();
            while (await reader.ReadAsync()) datas.Add(reader.GetByEntityStructure<PaymentOrderTransaction>());

            return datas;
        }

        public async Task<List<BankAccountList>> BankAccountList(int businessUnitId)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_BankAccountList @businessUnitId";
            command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

            using var reader = await command.ExecuteReaderAsync();

            List<BankAccountList> datas = new List<BankAccountList>();
            while (await reader.ReadAsync()) datas.Add(reader.GetByEntityStructure<BankAccountList>());

            return datas;
        }

        public async Task<bool> PaymentOrderPost(DataTable table, int journalNo, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    "SET NOCOUNT OFF EXEC SP_PaymentOrderPost @JournalNo,@UserId,@PaymentOrderTransactions";
                command.Parameters.AddWithValue(command, "@JournalNo", journalNo);
                command.Parameters.AddTableValue(command, "@PaymentOrderTransactions", "PaymentDocumentPost", table);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<(List<ASalfldg>, int)> PaymentOrderPostData(DataTable table, int allocationReference, int journalNo, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    @"SET NOCOUNT OFF EXEC SP_PaymentOrderPostData @JournalNo,@AllocationReference,@UserId,@PaymentOrderTransactions,@NewJournalNo = @NewJournalNo OUTPUT select @NewJournalNo as NewJournalNo";
                command.Parameters.AddWithValue(command, "@JournalNo", journalNo);
                command.Parameters.AddWithValue(command, "@AllocationReference", allocationReference);
                command.Parameters.AddTableValue(command, "@PaymentOrderTransactions", "PaymentDocumentPost", table);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@NewJournalNo", SqlDbType.Int);
                command.Parameters["@NewJournalNo"].Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                List<ASalfldg> datas = new List<ASalfldg>();
                while (await reader.ReadAsync())
                {
                    datas.Add(reader.GetByEntityStructure<ASalfldg>());
                }

                int newJournalNo = 0;
                if (datas.Count > 0)
                    newJournalNo = datas[0].JRNAL_NO;

                return (datas, newJournalNo);
            }
        }

        public ASalfldg GetSalfldg(DbDataReader reader)
        {
            return new ASalfldg
            {
                ACCNT_CODE = reader.Get<string>("AccountCode")
            };
        }

        public async Task<PaymentOrderPostMainSaveResult> PaymentOrderPostSaveMain(PaymentOrderPostMain paymentOrderMain, int allocationReference, int journalNo, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText =
                    @"SET NOCOUNT OFF EXEC SP_PaymentOrderMain_IUD @PaymentOrderMainId,@BusinessUnitId,@PaymentOrderNo,
                                                                   @VendorCode,@CurrencyCode,@PaymentDate,@BankAccount,
                                                                   @BankCharge,@BankChargeAccount,@Comment,@Amount,@JournalNo,
                                                                   @AllocationReference,@UserId,
                                                                   @NewPaymentOrderMainId = @NewPaymentOrderMainId OUTPUT,
                                                                   @NewPaymentOrderNo = @NewPaymentOrderNo OUTPUT 
                                                                   select @NewPaymentOrderMainId as NewPaymentOrderMainId,
                                                                   @NewPaymentOrderNo as NewPaymentOrderNo";

                command.Parameters.AddWithValue(command, "@PaymentOrderMainId", paymentOrderMain.PaymentOrderMainId);

                command.Parameters.AddWithValue(command, "@BusinessUnitId", paymentOrderMain.BusinessUnitId);

                command.Parameters.AddWithValue(command, "@PaymentOrderNo", paymentOrderMain.PaymentOrderNo);

                command.Parameters.AddWithValue(command, "@VendorCode", paymentOrderMain.VendorCode);

                command.Parameters.AddWithValue(command, "@CurrencyCode", paymentOrderMain.CurrencyCode);

                command.Parameters.AddWithValue(command, "@PaymentDate", paymentOrderMain.PaymentDate);

                command.Parameters.AddWithValue(command, "@BankAccount", paymentOrderMain.BankAccount);

                command.Parameters.AddWithValue(command, "@BankCharge", paymentOrderMain.BankCharge);

                command.Parameters.AddWithValue(command, "@BankChargeAccount", paymentOrderMain.BankChargeAccount);

                command.Parameters.AddWithValue(command, "@Comment", paymentOrderMain.Comment);

                command.Parameters.AddWithValue(command, "@Amount", paymentOrderMain.Amount);

                command.Parameters.AddWithValue(command, "@JournalNo", journalNo);

                command.Parameters.AddWithValue(command, "@AllocationReference", allocationReference);

                command.Parameters.AddWithValue(command, "@UserId", userId);

                command.Parameters.Add("@NewPaymentOrderMainId", SqlDbType.Int);
                command.Parameters["@NewPaymentOrderMainId"].Direction = ParameterDirection.Output;

                command.Parameters.Add("@NewPaymentOrderNo", SqlDbType.NVarChar, 15);
                command.Parameters["@NewPaymentOrderNo"].Direction = ParameterDirection.Output;

                PaymentOrderPostMainSaveResult saveResult = new PaymentOrderPostMainSaveResult();
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    saveResult.PaymentOrderMainId = reader.Get<int>("NewPaymentOrderMainId");
                    saveResult.PaymentOrderNo = reader.Get<string>("NewPaymentOrderNo");
                }

                return saveResult;
            }
        }

        public async Task<bool> PaymentOrderPostDetailSave(int paymentOrderMainId, DataTable detailData)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_PaymentOrderDetails_IUD @PaymentOrderMainId,@PaymentOrderDetails";
                command.Parameters.AddWithValue(command, "@PaymentOrderMainId", paymentOrderMainId);
                command.Parameters.AddTableValue(command, "@PaymentOrderDetails", "PaymentOrderDetailsType", detailData);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> PaymentOrderPostTransactionSave(int paymentOrderMainId, DataTable transactionData)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_PaymentOrderTransactin_IUD @PaymentOrderMainId,@PaymentOrderTransaction";
                command.Parameters.AddWithValue(command, "@PaymentOrderMainId", paymentOrderMainId);
                command.Parameters.AddTableValue(command, "@PaymentOrderTransaction", "PaymentOrderTransactionType", transactionData);
                var value = await command.ExecuteNonQueryAsync();
                return value > 0;
            }
        }

        public async Task<bool> PaymentOrderDetailsCheckNonAllocated(DataTable detailData)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_PaymentOrderDetailsCheckNonAllocated @PaymentOrderDetails";
                command.Parameters.AddTableValue(command, "@PaymentOrderDetails", "PaymentOrderDetailsType", detailData);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                    return true;

                return false;
            }
        }
    }
}
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IPaymentRepository
    {
        Task<List<CreateBalance>> CreateBalanceAsync(CreateBalanceModel createBalance);
        Task<List<CreateAdvance>> CreateAdvanceAsync(CreateAdvanceModel createBalance);
        Task<List<CreateOrder>> CreateOrderAsync(CreateOrderModel createOrder);
        Task<InfoHeader> InfoHeader(int paymentDocumentMainId);
        Task<List<InfoDetail>> InfoDetail(int paymentDocumentMainId);
        Task<List<InfoApproval>> InfoApproval(int paymentDocumentMainId);
        Task<List<AttachmentDto>> InfoAttachments(int paymentDocumentMainId);
        Task<List<InvoiceLink>> InvoiceLinks(int paymentDocumentMainId);
        Task<List<OrderLink>> OrderLinks(int paymentDocumentMainId);
        Task<List<BidComparisonLink>> BidComparisonLinks(int paymentDocumentMainId);
        Task<List<BidLink>> BidLinks(int paymentDocumentMainId);
        Task<List<RFQLink>> RFQLinks(int paymentDocumentMainId);
        Task<List<RequestLink>> RequestLinks(int paymentDocumentMainId);
        Task<List<All>> All(int userId, PaymentGetModel payment);
        Task<List<Approved>> Approved(int userId, PaymentGetModel payment);
        Task<List<Bank>> Bank(int userId, PaymentGetModel payment);
        Task<List<Draft>> Draft(int userId, PaymentGetModel payment);
        Task<List<Held>> Held(int userId, PaymentGetModel payment);
        Task<List<Rejected>> Rejected(int userId, PaymentGetModel payment);
        Task<List<WaitingForApproval>> WaitingForApproval(int userId, PaymentGetModel payment);
        Task<bool> SendToApprove(int userId, int paymentDocumentMainId);
        Task<PaymentDocumentSaveResultModel> MainSave(int userId, PaymentDocumentMainSaveModel model);
        Task<bool> DetailSave(DataTable model);
        Task<bool> Delete(int paymentDocumentMainId, int userId);
        Task<decimal> VendorBalance(int businessUnitId, string vendorCode);
        Task<bool> ChangeStatus(int userId, int paymentDocumentMainId, int sequence, int approveStatus, string comment);
        Task<List<AttachmentTypes>> AttachmentTypes();
        Task<List<PaymentRequest>> PaymentRequest(PaymentRequestGetModel model);
        Task<bool> PaymentOperation(int userId, DataTable table, PaymentOperations operation);
        Task<PaymentOrderMain> PaymentOrderMainLoad(PaymentOrderParamModel model);
        Task<List<PaymentOrderDetail>> PaymentOrderDetailLoad(PaymentOrderParamModel model);
        Task<List<PaymentOrderTransaction>> PaymentOrderTransaction(DataTable table, int paymentOrderMainId, DateTime paymentDate, string bankAccount, decimal bankCharge);
        Task<List<BankAccountList>> BankAccountList(int businessUnitId);
        Task<bool> PaymentOrderPost(DataTable table, int journalNo, int userId);
        Task<(List<ASalfldg>, int)> PaymentOrderPostData(DataTable table, int allocationReference, int journalNo, int userId);
        Task<PaymentOrderPostMainSaveResult> PaymentOrderPostSaveMain(PaymentOrderPostMain paymentOrderMain, int allocationReference, int journalNo, int userId);
        Task<bool> PaymentOrderPostDetailSave(int paymentOrderMainId, DataTable detailData);
        Task<bool> PaymentOrderPostTransactionSave(int paymentOrderMainId, DataTable transactionData);
        Task<bool> PaymentOrderDetailsCheckNonAllocated(DataTable detailData);
        Task<List<AllocationData>> PaymentOrderAllocationData(int paymentOrderMainId, int userId);
        Task<List<PaymentOrder>> PaymentOrders(PaymentOrderGetModel payment);
    }
}

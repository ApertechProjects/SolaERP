using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IPaymentService
    {
        Task<ApiResponse<List<CreateBalanceDto>>> CreateBalanceAsync(CreateBalanceModel createBalance);
        Task<ApiResponse<List<CreateAdvanceDto>>> CreateAdvanceAsync(CreateAdvanceModel createAdvance);
        Task<ApiResponse<List<CreateOrderDto>>> CreateOrderAsync(CreateOrderModel createOrder);
        Task<ApiResponse<PaymentInfoModel>> Info(int paymentDocumentMainId);
        ApiResponse<List<AttachmentTypes>> DocumentTypes();
        Task<ApiResponse<List<WaitingForApprovalDto>>> WaitingForApproval(string name, PaymentGetModel payment);
        Task<ApiResponse<List<AllDto>>> All(string name, PaymentGetModel payment);
        Task<ApiResponse<List<DraftDto>>> Draft(string name, PaymentGetModel payment);
        Task<ApiResponse<List<ApprovedDto>>> Approved(string name, PaymentGetModel payment);
        Task<ApiResponse<List<HeldDto>>> Held(string name, PaymentGetModel payment);
        Task<ApiResponse<List<RejectedDto>>> Rejected(string name, PaymentGetModel payment);
        Task<ApiResponse<List<BankDto>>> Bank(string name, PaymentGetModel payment);
        Task<ApiResponse<bool>> SendToApprove(string name, int paymentDocumentMainId);
        Task<ApiResponse<PaymentDocumentSaveResultModel>> Save(string name, PaymentDocumentSaveModel model);
        Task<ApiResponse<bool>> Delete(int paymentDocumentMainId);
        Task<ApiResponse<decimal>> VendorBalance(int businessUnitId, string vendorCode);
        Task<ApiResponse<List<AttachmentDto>>> Attachments(int paymentDocumentMainId);
        Task<ApiResponse<bool>> ChangeStatus(string name, PaymentChangeStatusModel model);
        Task<ApiResponse<List<CreateDocumentDto>>> CreateDocument(PaymentCreateDocumentModel model);
    }
}

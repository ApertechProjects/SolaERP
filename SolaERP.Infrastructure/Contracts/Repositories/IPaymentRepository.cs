using SolaERP.Application.Entities.Payment;
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
        Task<bool> Delete(int paymentDocumentMainId);
        Task<decimal> VendorBalance(int businessUnitId, string vendorCode);
    }
}

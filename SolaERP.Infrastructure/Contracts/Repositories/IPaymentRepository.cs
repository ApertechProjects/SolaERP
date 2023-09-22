using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

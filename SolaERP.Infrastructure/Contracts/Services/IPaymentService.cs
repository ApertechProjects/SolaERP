using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IPaymentService
    {
        Task<ApiResponse<List<CreateBalanceDto>>> CreateBalanceAsync(CreateBalanceModel createBalance);
        Task<ApiResponse<List<CreateAdvanceDto>>> CreateAdvanceAsync(CreateAdvanceModel createAdvance);
        Task<ApiResponse<List<CreateOrderDto>>> CreateOrderAsync(CreateOrderModel createOrder);
        Task<ApiResponse<PaymentInfoModel>> Info(int paymentDocumentMainId);
    }
}

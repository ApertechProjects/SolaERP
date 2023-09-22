using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
        }

        public async Task<ApiResponse<List<CreateAdvanceDto>>> CreateAdvanceAsync(CreateAdvanceModel createAdvance)
        {
            var data = await _paymentRepository.CreateAdvanceAsync(createAdvance);
            var dto = _mapper.Map<List<CreateAdvanceDto>>(data);
            return ApiResponse<List<CreateAdvanceDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<CreateBalanceDto>>> CreateBalanceAsync(CreateBalanceModel createBalance)
        {
            var data = await _paymentRepository.CreateBalanceAsync(createBalance);
            var dto = _mapper.Map<List<CreateBalanceDto>>(data);
            return ApiResponse<List<CreateBalanceDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<CreateOrderDto>>> CreateOrderAsync(CreateOrderModel createOrder)
        {
            var data = await _paymentRepository.CreateOrderAsync(createOrder);
            var dto = _mapper.Map<List<CreateOrderDto>>(data);
            return ApiResponse<List<CreateOrderDto>>.Success(dto);
        }

        public async Task<ApiResponse<PaymentInfoModel>> Info(int paymentDocumentMainId)
        {
            var header = await _paymentRepository.InfoHeader(paymentDocumentMainId);
            var headerDto = _mapper.Map<InfoHeaderDto>(header);
            var details = await _paymentRepository.InfoDetail(paymentDocumentMainId);
            var detailsDto = _mapper.Map<List<InfoDetailDto>>(details);
            var approvalInformation = await _paymentRepository.InfoApproval(paymentDocumentMainId);
            var approvalDto = _mapper.Map<List<InfoApproval>>(approvalInformation);
            return ApiResponse<PaymentInfoModel>.Success(new PaymentInfoModel
            {
                InfoApproval = approvalDto,
                InfoDetail = detailsDto,
                InfoHeader = headerDto,
            });
        }
    }
}

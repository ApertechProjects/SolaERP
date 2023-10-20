using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IUserRepository userRepository, IMapper mapper, IInvoiceRepository invoiceRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _invoiceRepository.RegisterAll(model, userId);
            var dto = _mapper.Map<List<RegisterAllDto>>(data);
            return ApiResponse<List<RegisterAllDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _invoiceRepository.RegisterWFA(model, userId);
            var dto = _mapper.Map<List<RegisterWFADto>>(data);
            return ApiResponse<List<RegisterWFADto>>.Success(dto);
        }
    }
}

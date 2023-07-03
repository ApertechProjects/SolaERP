using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.ViewModels;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ISupplierEvaluationRepository _supplierEvaluationRepository;

        public VendorService(IVendorRepository vendorRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ISupplierEvaluationRepository supplierEvaluationRepository)
        {
            _repository = vendorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _supplierEvaluationRepository = supplierEvaluationRepository;
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(string userIdentity, string tax, int status)
        {
            var taxId = await GetByTaxIdAsync(tax);
            var userId = await _userRepository.ConvertIdentity(userIdentity);
            var result = await _repository.ChangeStatusAsync(taxId, status, userId);
            if (result)
                return ApiResponse<bool>.Success(result);
            else
                return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity, VendorFilter filter, Status status, ApprovalStatus approval)
        {
            List<VendorAll> allVendors = await _repository.GetAll(Convert.ToInt32(userIdentity),
                filter, (int)status, (int)approval);

            List<VendorAllDto> dto = _mapper.Map<List<VendorAllDto>>(allVendors);
            return ApiResponse<List<VendorAllDto>>.Success(dto, 200);
        }

        public async Task<VendorInfo> GetByTaxAsync(string taxId)
            => await _repository.GetByTaxAsync(taxId);

        public async Task<int> GetByTaxIdAsync(string taxId)
        {
            VendorInfo entity = await _repository.GetByTaxAsync(taxId);
            return entity.VendorId;
        }

        public async Task<ApiResponse<List<VendorAll>>> GetDraftAsync(string userIdentity, VendorFilter filter)
        {
            var data = await _repository.GetDraftAsync(Convert.ToInt32(userIdentity), filter);
            return ApiResponse<List<VendorAll>>.Success(data, 200);
        }

        public async Task<ApiResponse<VM_GetVendorFilters>> GetFiltersAsync()
        {
            var prequalificationTypesTask = _supplierEvaluationRepository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _supplierEvaluationRepository.GetBusinessCategoriesAsync();
            var productServicesTask = _supplierEvaluationRepository.GetProductServicesAsync();

            await Task.WhenAll(prequalificationTypesTask, businessCategoriesTask, productServicesTask);
            VM_GetVendorFilters viewModel = new()
            {
                PrequalificationCategories = prequalificationTypesTask.Result,
                BusinessCategories = businessCategoriesTask.Result,
                ProductServices = productServicesTask.Result,
            };

            return ApiResponse<VM_GetVendorFilters>.Success(viewModel, 200);
        }

        public async Task<ApiResponse<List<VendorWFADto>>> GetHeldAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetHeld(Convert.ToInt32(userIdentity), filter);
            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
        }

        public async Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetWFAAsync(Convert.ToInt32(userIdentity), filter);
            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
        }
    }
}

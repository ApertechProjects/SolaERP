using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Vendors;
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

        public async Task<VendorInfo> GetByTaxAsync(string taxId)
            => await _repository.GetByTaxAsync(taxId);

        public async Task<int> GetByTaxIdAsync(string taxId)
        {
            VendorInfo entity = await _repository.GetByTaxAsync(taxId);
            return entity.VendorId;
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

        public Task<ApiResponse<List<VendorWFA>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}

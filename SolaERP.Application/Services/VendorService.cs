using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ViewModels;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repository;
        private readonly ICurrencyCodeRepository _currencyCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierEvaluationRepository _supplierEvaluationRepository;

        public VendorService(IVendorRepository vendorRepository,
            ICurrencyCodeRepository currencyCodeRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ISupplierEvaluationRepository supplierEvaluationRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = vendorRepository;
            _currencyCodeRepository = currencyCodeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _supplierEvaluationRepository = supplierEvaluationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            model.UserId = user.Id;
            return ApiResponse<bool>.Success(await _repository.ApproveAsync(model), 200);
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(TaxModel taxModel, string userIdentity)
        {
            var userId = await _userRepository.ConvertIdentity(userIdentity);
            int counter = 0;
            for (int i = 0; i < taxModel.TaxIds.Count; i++)
            {
                var taxId = await GetByTaxIdAsync(taxModel.TaxIds[i]);
                var result = await _repository.ChangeStatusAsync(taxId, taxModel.Status, userId);
                if (result)
                    counter++;
            }


            if (counter == taxModel.TaxIds.Count)
                return ApiResponse<bool>.Success(200);
            else
                return ApiResponse<bool>.Fail("Problem detected", 400);
        }
        public async Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity, VendorAllCommandRequest request)
        {
            List<VendorAll> allVendors = await _repository.GetAll(Convert.ToInt32(userIdentity), request);

            List<VendorAllDto> dto = _mapper.Map<List<VendorAllDto>>(allVendors);
            return ApiResponse<List<VendorAllDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<VendorAllDto>>> GetApprovedAsync(string userIdentity)
        {
            var approvedByCurrentUser = await _repository.GetApprovedAsync(Convert.ToInt32(userIdentity));
            var dto = _mapper.Map<List<VendorAllDto>>(approvedByCurrentUser);

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
            List<VendorWFA> vendorWFAs = await _repository.GetHeldAsync(Convert.ToInt32(userIdentity), filter);
            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
        }
        public async Task<ApiResponse<List<VendorWFADto>>> GetRejectedAsync(string userIdentity, VendorFilter filter)
        {
            var rejectedByCurrentUser = await _repository.GetRejectedAsync(Convert.ToInt32(userIdentity), filter);
            var rejectedByCurrentUserDto = _mapper.Map<List<VendorWFADto>>(rejectedByCurrentUser);

            return ApiResponse<List<VendorWFADto>>.Success(rejectedByCurrentUserDto, 200);
        }

        public async Task<ApiResponse<VendorGetModel>> GetVendorCard(int vendorId)
        {
            var header = _mapper.Map<VendorCardDto>(await _repository.GetHeader(vendorId));
            var currency = string.IsNullOrEmpty(header.BusinessUnitCode) ? new List<Application.Entities.Currency.Currency>() : await _currencyCodeRepository.CurrencyCodes(header.BusinessUnitCode);
            var paymentTerms = _supplierEvaluationRepository.GetPaymentTermsAsync();
            var deliveryTerms = _supplierEvaluationRepository.GetDeliveryTermsAsync();
            var bankDetails = _supplierEvaluationRepository.GetVendorBankDetailsAsync(vendorId);
            var businessCategory = _supplierEvaluationRepository.GetVendorBuCategoriesAsync(vendorId);
            var users = _supplierEvaluationRepository.GetVendorUsers(vendorId);
            var score = _supplierEvaluationRepository.Scores(vendorId);
            var shipment = _supplierEvaluationRepository.Shipments();
            var withHoldingTax = _supplierEvaluationRepository.WithHoldingTaxDatas();
            var tax = _supplierEvaluationRepository.TaxDatas();
            var itemCategories = _supplierEvaluationRepository.GetBusinessCategoriesAsync();
            await Task.WhenAll
                (
                    paymentTerms,
                    deliveryTerms,
                    bankDetails,
                    users,
                    shipment,
                    withHoldingTax,
                    tax,
                    users
                );

            VendorGetModel vendorModel = new VendorGetModel()
            {
                Header = header,
                Currencies = currency,
                PaymentTerms = paymentTerms.Result,
                DeliveryTerms = deliveryTerms.Result,
                VendorBankDetails = bankDetails.Result.Select(x => new VendorBankDetailDto { Bank = x.Bank, Currency = x.Currency, AccountNumber = x.AccountNumber }).ToList(),
                VendorUsers = users.Result,
                VendorBuCategories = businessCategory.Result,
                Score = score.Result,
                Shipments = shipment.Result,
                WithHoldingTaxDatas = withHoldingTax.Result,
                TaxDatas = tax.Result,
            };

            return ApiResponse<VendorGetModel>.Success(vendorModel);
        }
        public async Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetWFAAsync(Convert.ToInt32(userIdentity), filter);
            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
        }

        public async Task<ApiResponse<bool>> SendToApproveAsync(VendorSendToApproveRequest request)
        {
            bool isSuccessFull = await _repository.SendToApprove(request);
            await _unitOfWork.SaveChangesAsync();

            return isSuccessFull ? ApiResponse<bool>.Success(isSuccessFull, 200)
                :
                ApiResponse<bool>.Success(isSuccessFull, 200);
        }


    }
}

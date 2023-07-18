using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ViewModels;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierEvaluationRepository _supplierRepository;
        private readonly IAttachmentRepository _attachment;

        public VendorService(IVendorRepository vendorRepository,
                             IUserRepository userRepository,
                             IMapper mapper,
                             ISupplierEvaluationRepository supplierRepository,
                             IUnitOfWork unitOfWork,
                             IAttachmentRepository attachment)
        {
            _repository = vendorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _attachment = attachment;
        }

        public async Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            model.UserId = user.Id;
            return ApiResponse<bool>.Success(await _repository.ApproveAsync(model), 200);
        }
        public async Task<ApiResponse<bool>> ChangeStatusAsync(VendorStatusModel taxModel, string userIdentity)
        {
            var userId = await _userRepository.ConvertIdentity(userIdentity);
            int counter = 0;
            for (int i = 0; i < taxModel.VendorIds.Count; i++)
            {
                var taxId = await GetByTaxIdAsync(taxModel.VendorIds[i]);
                var result = await _repository.ChangeStatusAsync(taxId, taxModel.Status, userId);
                if (result)
                    counter++;
            }


            if (counter == taxModel.VendorIds.Count)
                return ApiResponse<bool>.Success(200);
            else
                return ApiResponse<bool>.Fail("Problem detected", 400);
        }
        public async Task<ApiResponse<bool>> DeleteAsync(string userIdentity, VendorDeleteModel model)
        {
            int counter = 0;
            for (int i = 0; i < model.Ids.Count; i++)
            {
                var operationResult = await _repository.DeleteAsync(Convert.ToInt32(userIdentity), model.Ids[0]);
                bool isSuccessfull = operationResult != 0;
                if (isSuccessfull)
                    counter++;
            }

            await _unitOfWork.SaveChangesAsync();
            if (counter == model.Ids.Count)
                return ApiResponse<bool>.Success(true, 200);
            else
                return ApiResponse<bool>.Success("some datas can not be deleted");
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
            var prequalificationTypesTask = _supplierRepository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _supplierRepository.GetBusinessCategoriesAsync();
            var productServicesTask = _supplierRepository.GetProductServicesAsync();

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
        public async Task<ApiResponse<VM_VendorCard>> GetAsync(int vendorId)
        {
            var header = _mapper.Map<VendorCardDto>(await _repository.GetHeader(vendorId));

            var paymentTerms = _supplierRepository.GetPaymentTermsAsync();
            var deliveryTerms = _supplierRepository.GetDeliveryTermsAsync();
            var currency = _supplierRepository.GetCurrenciesAsync();
            var bankDetails = _supplierRepository.GetVendorBankDetailsAsync(vendorId);
            var vendorBuCategories = _supplierRepository.GetVendorBuCategoriesAsync(vendorId);
            var buCategories = _supplierRepository.GetBusinessCategoriesAsync();
            var users = _supplierRepository.GetVendorUsers(vendorId);
            var score = _supplierRepository.Scores(vendorId);
            var shipment = _supplierRepository.Shipments();
            var withHoldingTax = _supplierRepository.WithHoldingTaxDatas();
            var tax = _supplierRepository.TaxDatas();
            var itemCategories = _supplierRepository.GetBusinessCategoriesAsync();
            var countries = _supplierRepository.GetCountriesAsync();

            await Task.WhenAll
                  (
                    paymentTerms,
                    deliveryTerms,
                    bankDetails,
                    users,
                    shipment,
                    withHoldingTax,
                    tax,
                    users,
                    currency,
                    buCategories,
                    countries
                  );



            var matchedBuCategories = buCategories.Result
                .Where(x => vendorBuCategories.Result.Select(y => y.BusinessCategoryId).Contains(x.Id))
                .ToList();


            VM_VendorCard vendorModel = new VM_VendorCard()
            {
                Header = header,
                Currencies = currency.Result,
                PaymentTerms = paymentTerms.Result,
                DeliveryTerms = deliveryTerms.Result,
                VendorBankDetails = bankDetails.Result,
                VendorUsers = users.Result,
                ItemCategories = matchedBuCategories,
                Score = score.Result,
                Shipments = shipment.Result,
                WithHoldingTaxDatas = withHoldingTax.Result,
                TaxDatas = tax.Result,
            };

            return ApiResponse<VM_VendorCard>.Success(vendorModel);
        }
        public async Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetWFAAsync(Convert.ToInt32(userIdentity), filter);

            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
        }

        public async Task<ApiResponse<bool>> SaveAsync(string userIdentity, VendorCardDto vendorDto)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<Task<bool>> tasks = new();
            Vendor vendor = _mapper.Map<Vendor>(vendorDto);

            int userId = Convert.ToInt32(userIdentity);
            int vendorId = 0;

            vendorId = vendor.IsNewVendor() ? await _repository.AddAsync(userId, vendor) :
                                              await _repository.UpdateAsync(userId, vendor);

            await _supplierRepository.DeleteRepresentedProductAsync(vendorId);
            await _supplierRepository.DeleteRepresentedCompanyAsync(vendorId);

            await _supplierRepository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany { VendorId = vendorId, RepresentedCompanyName = string.Join(",", vendor?.RepresentedCompanies) });
            await _supplierRepository.AddRepresentedProductAsync(new RepresentedProductData { VendorId = vendorId, RepresentedProductName = string.Join(",", vendor?.RepresentedProducts) });


            if (vendorDto.Logo is not null)
            {
                var vendorLogo = _mapper.Map<AttachmentSaveModel>(vendorDto.Logo);

                vendorLogo.SourceId = vendorId;
                vendorLogo.SourceType = SourceType.VEN_LOGO.ToString();

                await _attachment.SaveAttachmentAsync(vendorLogo);
            }

            foreach (var x in vendorDto.BankAccounts)
            {
                if (x.Type == 2 && x.Id > 0)
                    await _repository.DeleteBankDetailsAsync(user.Id, x.Id);
                else
                {
                    var detaildId = await _repository.UpdateBankDetailsAsync(user.Id, _mapper.Map<VendorBankDetail>(x));
                    x.VendorId = vendorId;

                    if (x.AccountVerificationLetter != null)
                    {
                        tasks.AddRange(x.AccountVerificationLetter.Select(attachment => //+
                        {
                            if (attachment.Type == 2 && attachment.AttachmentId > 0)
                                return _attachment.DeleteAttachmentAsync(attachment.AttachmentId);
                            else
                            {
                                var entity = _mapper.Map<AttachmentSaveModel>(attachment);
                                entity.SourceId = detaildId;
                                entity.SourceType = SourceType.VEN_BNK.ToString();
                                return _attachment.SaveAttachmentAsync(entity);
                            }
                        }));
                    }
                }
            }
            await Task.WhenAll(tasks);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<Vendor>.CreateApiResponse(x => x.VendorId > 0, vendor);
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

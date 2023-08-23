using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ViewModels;
using SolaERP.DataAccess.Extensions;

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
        private readonly IFileUploadService _fileUploadService;
        private readonly IGeneralRepository _generalRepository;
        public VendorService(IVendorRepository vendorRepository,
                             IUserRepository userRepository,
                             IMapper mapper,
                             ISupplierEvaluationRepository supplierRepository,
                             IUnitOfWork unitOfWork,
                             IAttachmentRepository attachment,
                             IFileUploadService fileUploadService,
                             IGeneralRepository generalRepository)
        {
            _repository = vendorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _attachment = attachment;
            _fileUploadService = fileUploadService;
            _generalRepository = generalRepository;
        }

        public async Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            model.UserId = user.Id;
            var result = await _repository.ApproveAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
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
            allVendors = allVendors.GetDataByFilter(request.Text);

            List<VendorAllDto> dto = _mapper.Map<List<VendorAllDto>>(allVendors);
            if (dto.Count > 0)
                return ApiResponse<List<VendorAllDto>>.Success(dto, 200);
            return ApiResponse<List<VendorAllDto>>.Fail("Data not found", 404);
        }
        public async Task<ApiResponse<List<VendorAllDto>>> GetApprovedAsync(string userIdentity, string text)
        {
            var approvedByCurrentUser = await _repository.GetApprovedAsync(Convert.ToInt32(userIdentity));
            approvedByCurrentUser = approvedByCurrentUser.GetDataByFilter(text);

            var dto = _mapper.Map<List<VendorAllDto>>(approvedByCurrentUser);
            if (dto.Count > 0)
                return ApiResponse<List<VendorAllDto>>.Success(dto, 200);
            return ApiResponse<List<VendorAllDto>>.Fail("Data not found", 404);
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
            data = data.GetDataByFilter(filter.Text);
            if (data.Count > 0)
                return ApiResponse<List<VendorAll>>.Success(data, 200);
            return ApiResponse<List<VendorAll>>.Fail("Data not found", 404);
        }
        public async Task<ApiResponse<VM_GetVendorFilters>> GetFiltersAsync()
        {
            var prequalificationTypesTask = _supplierRepository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _generalRepository.BusinessCategories();
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
            vendorWFAs = vendorWFAs.GetDataByFilter(filter.Text);

            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            if (vendorAllDtos.Count > 0)
                return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);
        }
        public async Task<ApiResponse<List<VendorWFADto>>> GetRejectedAsync(string userIdentity, VendorFilter filter)
        {
            var rejectedByCurrentUser = await _repository.GetRejectedAsync(Convert.ToInt32(userIdentity), filter);
            rejectedByCurrentUser = rejectedByCurrentUser.GetDataByFilter(filter.Text);
            var rejectedByCurrentUserDto = _mapper.Map<List<VendorWFADto>>(rejectedByCurrentUser);
            if (rejectedByCurrentUserDto.Count > 0)
                return ApiResponse<List<VendorWFADto>>.Success(rejectedByCurrentUserDto, 200);

            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);
        }
        public async Task<ApiResponse<VM_VendorCard>> GetAsync(int vendorId)
        {
            var header = _mapper.Map<VendorLoadDto>(await _repository.GetHeader(vendorId));
            header.Logo = _fileUploadService.GetFileLink(header.Logo, Modules.Vendors);

            var paymentTerms = _supplierRepository.GetPaymentTermsAsync();
            var deliveryTerms = _supplierRepository.GetDeliveryTermsAsync();
            var currency = _supplierRepository.GetCurrenciesAsync();
            var bankDetails = _supplierRepository.GetVendorBankDetailsAsync(vendorId);
            var vendorBuCategories = _supplierRepository.GetVendorBuCategoriesAsync(vendorId);
            var buCategories = _generalRepository.BusinessCategories();
            var users = _supplierRepository.GetVendorUsers(vendorId);
            var score = _supplierRepository.Scores(vendorId);
            var shipment = _supplierRepository.Shipments();
            var withHoldingTax = _supplierRepository.WithHoldingTaxDatas();
            var tax = _supplierRepository.TaxDatas();
            var itemCategories = _generalRepository.BusinessCategories();
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
                Countries = countries.Result,
            };

            return ApiResponse<VM_VendorCard>.Success(vendorModel);
        }
        public async Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetWFAAsync(Convert.ToInt32(userIdentity), filter);

            if (!string.IsNullOrEmpty(filter.Text?.ToString()))
            {
                vendorWFAs = vendorWFAs.GetDataByFilter(filter.Text);
            }

            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            if (vendorWFAs.Count > 0)
                return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);

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

            string vendorLogo = await _repository.GetVendorLogo(vendorId);
            vendor.Logo = await _fileUploadService.GetLinkForEntity(vendorDto.LogoFile.File, vendorDto.CheckLogoIsDeleted, vendorLogo);


            await _attachment.DeleteAttachmentAsync(vendorId, SourceType.VEN_LOGO);
            var vendorLogoData = _mapper.Map<AttachmentSaveModel>(vendorDto.LogoFile);

            if (!vendorDto.CheckLogoIsDeleted)
            {
                vendorLogoData.SourceId = vendorId;
                vendorLogoData.SourceType = SourceType.VEN_LOGO.ToString();
                vendorLogoData.FileLink = vendor.Logo;
                await _attachment.SaveAttachmentAsync(vendorLogoData);
            }
            if (vendorDto.BankAccounts is not null)
                foreach (var x in vendorDto?.BankAccounts)
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
            return vendorId > 0 ? ApiResponse<bool>.Success(true, 200) :
                                  ApiResponse<bool>.Success(false, 400);
        }

        public async Task<ApiResponse<bool>> SendToApproveAsync(VendorSendToApproveRequest request)
        {
            bool isSuccessFull = await _repository.SendToApprove(request);
            await _unitOfWork.SaveChangesAsync();

            return isSuccessFull ? ApiResponse<bool>.Success(isSuccessFull, 200) :
                ApiResponse<bool>.Success(isSuccessFull, 200);
        }

        public async Task<ApiResponse<List<VendorInfoDto>>> Vendors(string userIdentity)
        {
            int userId = await _userRepository.ConvertIdentity(userIdentity);
            var data = await _repository.Vendors(userId);
            var dto = _mapper.Map<List<VendorInfoDto>>(data);
            return ApiResponse<List<VendorInfoDto>>.Success(dto, 200);
        }
    }
}

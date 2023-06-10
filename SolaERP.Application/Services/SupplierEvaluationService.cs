using AutoMapper;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.ColorSpaces;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.Shared;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class SupplierEvaluationService : ISupplierEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _buRepository;
        private readonly ISupplierEvaluationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IStorage _storage;
        private readonly IOptions<StorageOption> _storageOption;

        public SupplierEvaluationService(ISupplierEvaluationRepository repository,
                                         IMapper mapper,
                                         IUnitOfWork unitOfWork,
                                         IBusinessUnitRepository buRepository,
                                         IUserRepository userRepository,
                                         IVendorRepository vendorRepository,
                                         IAttachmentRepository attachmentRepository,
                                         IStorage storage,
                                         IOptions<StorageOption> storageOption)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _buRepository = buRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _vendorRepository = vendorRepository;
            _attachmentRepository = attachmentRepository;
            _storage = storage;
            _storageOption = storageOption;
        }

        public async Task<ApiResponse<bool>> AddAsync(string useridentity, SupplierRegisterCommand command)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(useridentity));
            int vendorId = await _vendorRepository.AddVendorAsync(user.Id, _mapper.Map<Vendor>(command.CompanyInfo));

            command.DueDiligence.VendorId = vendorId;
            command.Prequalification.VendorId = vendorId;
            command.CodeOfBuConduct.ForEach(x => x.VendorId = vendorId);
            command.NonDisclosureAgreement.ForEach(x => x.VendorId = vendorId);
            command.BankDetails.ForEach(x => x.VendorId = vendorId);

            List<Task<bool>> tasks = new()
            {
                _repository.AddDueDesignAsync(command.DueDiligence),
            };

            tasks.AddRange(command.NonDisclosureAgreement.Select(x => _repository.AddNDAAsync(_mapper.Map<VendorNDA>(x))));
            tasks.AddRange(command.CodeOfBuConduct.Select(x => _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(x))));
            tasks.AddRange(command.BankDetails.Select(x => _vendorRepository.AddBankDetailsAsync(user.Id, _mapper.Map<VendorBankDetail>(x))));

            await Task.WhenAll(tasks);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(tasks.All(x => x.Result), 200);
        }

        public async Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model)
        {
            CompanyInformation companyInformation = new()
            {
                BusinessCategories = await _repository.GetBusinessCategoriesAsync(),
                PaymentTerms = await _repository.GetPaymentTermsAsync(),
                Countries = await _repository.GetCountriesAsync(),
                PrequalificationTypes = await _repository.GetPrequalificationCategoriesAsync(),
                Services = await _repository.GetProductServicesAsync(),
            };
            BankCodesDto bankCodes = new()
            {
                Currencies = await _repository.GetCurrenciesAsync(),
                BankDetails = await _repository.GetVondorBankDetailsAsync(model.VendorId),
            };
            List<BusinessUnits> buUnits = await _buRepository.GetAllAsync();

            VM_GET_SupplierEvaluation viewModel = new()
            {
                CompanyInformation = companyInformation,
                BankCodes = bankCodes,
                BusinessUnits = _mapper.Map<List<BusinessUnitsDto>>(buUnits),
                DueDiligenceDesign = await GetDueDesignsAsync("", model.Language)
            };

            return ApiResponse<VM_GET_SupplierEvaluation>.Success(viewModel, 200);
        }
        public async Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity)
        {
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            var currencyTask = _repository.GetCurrenciesAsync();
            var bankDetailsTask = _repository.GetVondorBankDetailsAsync(user.VendorId);
            var attachemtsTask = _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_BNK.ToString());

            await Task.WhenAll(currencyTask, bankDetailsTask);


            VM_GET_VendorBankDetails bankDetails = new()
            {
                Currencies = currencyTask.Result,
                BankDetails = _mapper.Map<List<VendorBankDetailDto>>(bankDetailsTask.Result),
                AccountVerificationLetter = _mapper.Map<List<AttachmentDto>>(attachemtsTask.Result),
            };

            return ApiResponse<VM_GET_VendorBankDetails>.Success(bankDetails, 200);
        }

        public async Task<ApiResponse<List<CodeOfBuConduct>>> GetCOBCAsync(string userIdentity)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<VendorCOBC> cobc = await _repository.GetCOBCAsync(user.VendorId);

            var buUnits = await _buRepository.GetAllAsync();
            var matchingBuUnitsIds = cobc.Select(x => x.BusinessUnitId).ToList();

            var result = buUnits
                        .Select(x => new CodeOfBuConduct
                        {
                            VendorFullName = user.FullName,
                            CobcID = cobc.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorCOBCId,
                            VendorId = user.VendorId,
                            BusinessUnitId = x.BusinessUnitId,
                            BusinessUnitCode = x.BusinessUnitCode,
                            BusinessUnitName = x.BusinessUnitName,
                            TaxId = x.TaxId,
                            Address = x.Address,
                            CountryCode = x.CountryCode,
                            FullName = x.FullName,
                            Position = x.Position,
                            IsAgreed = matchingBuUnitsIds.Contains(x.BusinessUnitId)
                        })
                        .ToList();


            return ApiResponse<List<CodeOfBuConduct>>.Success(result, 200);
        }

        public async Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(string userIdentity, string acceptLanguage)
             => ApiResponse<List<DueDiligenceDesignDto>>.Success(await GetDueDesignsAsync(userIdentity, Language.en));



        public async Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity)
        {
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            var vendorPrequalificationTask = _repository.GetVendorPrequalificationAsync(user.VendorId);
            var prequalificationTypesTask = _repository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _repository.GetBusinessCategoriesAsync();
            var vendorBusinessCategoriesTask = _repository.GetVendorBuCategoriesAsync(user.VendorId);
            var companyInfoTask = _repository.GetCompanyInfoAsync(user.VendorId);
            var vendorProductsTask = _repository.GetVendorProductServices(user.VendorId);
            var attachmentTask = _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_LOGO.ToString());
            var productServicesTask = _repository.GetProductServicesAsync();

            await Task.WhenAll(vendorPrequalificationTask,
                                prequalificationTypesTask,
                                businessCategoriesTask,
                                vendorBusinessCategoriesTask,
                                productServicesTask,
                                vendorProductsTask,
                                attachmentTask,
                                companyInfoTask);

            var matchedPrequalificationTypes = prequalificationTypesTask.Result
                .Where(x => vendorPrequalificationTask.Result.Select(y => y.PrequalificationCategoryId).Contains(x.Id))
                .ToList();

            var matchedBuCategories = businessCategoriesTask.Result
                .Where(x => vendorBusinessCategoriesTask.Result.Select(y => y.VendorBusinessCategoryId).Contains(x.Id))
                .ToList();

            var matchedProductServices = productServicesTask.Result
                 .Where(x => vendorProductsTask.Result.Select(y => y.ProductServiceId).Contains(x.Id))
                 .ToList();

            CompanyInfoDto companyInfo = _mapper.Map<CompanyInfoDto>(companyInfoTask.Result);
            companyInfo.PrequalificationCategories = matchedPrequalificationTypes;
            companyInfo.BusinessCategories = matchedBuCategories;
            companyInfo.Attachments = _mapper.Map<List<AttachmentDto>>(attachmentTask.Result);
            companyInfo.ProductServices = matchedProductServices;

            VM_GET_InitalRegistration viewModel = new()
            {
                CompanyInformation = companyInfo,
                BusinessCategories = businessCategoriesTask.Result,
                PaymentTerms = await _repository.GetPaymentTermsAsync(),
                PrequalificationTypes = prequalificationTypesTask.Result,
                Services = await _repository.GetProductServicesAsync(),
                ContactPerson = _mapper.Map<ContactPersonDto>(user),
            };

            return ApiResponse<VM_GET_InitalRegistration>.Success(viewModel, 200);
        }

        public async Task<ApiResponse<List<NonDisclosureAgreement>>> GetNDAAsync(string userIdentity)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<VendorNDA> nda = await _repository.GetNDAAsync(user.VendorId);

            var buUnits = await _buRepository.GetAllAsync();
            var matchingBuUnitsIds = nda.Select(y => y.BusinessUnitId).ToList();

            var result = buUnits
                .Select(x => new NonDisclosureAgreement
                {
                    VendorFullName = user.FullName,
                    NdaID = nda.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorNDAId,
                    VendorId = user.VendorId,
                    BusinessUnitId = x.BusinessUnitId,
                    BusinessUnitCode = x.BusinessUnitCode,
                    BusinessUnitName = x.BusinessUnitName,
                    TaxId = x.TaxId,
                    Address = x.Address,
                    CountryCode = x.CountryCode,
                    FullName = x.FullName,
                    Position = x.Position,
                    IsAgreed = matchingBuUnitsIds.Contains(x.BusinessUnitId)
                })
                .ToList();

            return ApiResponse<List<NonDisclosureAgreement>>.Success(result, 200);
        }

        public async Task<ApiResponse<PrequalificationDto>> GetPrequalificationAsync(string userIdentity)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            Prequalification pre = await _repository.GetPrequalificationAsync(user.VendorId);

            var result = _mapper.Map<PrequalificationDto>(pre);
            return ApiResponse<PrequalificationDto>.Success(result, 200);
        }

        private async Task<List<DueDiligenceDesignDto>> GetDueDesignsAsync(string userIdentity, Language language)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<DueDiligenceDesign> dueDiligence = await _repository.GetDueDiligencesDesignAsync(language);
            List<Application.Entities.SupplierEvaluation.VendorDueDiligence> dueDiligenceValues = await _repository.GetVendorDuesAsync(user.VendorId);

            var groupedList = dueDiligence.GroupBy(x => x.Title).ToList();
            var responseModel = new List<DueDiligenceDesignDto>();

            foreach (var group in groupedList)
            {
                var dto = new DueDiligenceDesignDto { Title = group.Key, Childs = new List<DueDiligenceChildDto>() };
                foreach (var d in group)
                {
                    var correspondingValue = dueDiligenceValues.FirstOrDefault(v => v.DueDiligenceDesignId == d.DesignId);
                    var childDto = new DueDiligenceChildDto
                    {
                        DesignId = d.DesignId,
                        LineNo = d.LineNo,
                        Question = d.Question,
                        HasTextBox = d.HasTextBox > 0 ? true : null,
                        TextboxValue = d.HasTextBox > 0 ? correspondingValue?.TextboxValue ?? "" : null,
                        CheckboxValue = d.HasCheckBox > 0 ? correspondingValue?.CheckboxValue : null,
                        RadioboxValue = d.HasRadioBox > 0 ? correspondingValue?.RadioboxValue : null,
                        IntValue = d.HasInt > 0 ? correspondingValue?.IntValue : null,
                        DecimalValue = d.HasDecimal > 0 ? correspondingValue?.DecimalValue : null,
                        DateTimeValue = d.HasDateTime > 0 ? correspondingValue?.DateTimeValue : null,
                        TextareaValue = d.HasTexArea > 0 ? correspondingValue?.TextareaValue ?? "" : null,
                        AgreementValue = d.HasAgreement ? correspondingValue?.AgreementValue : null,
                        AgreementText = d.HasAgreement ? d.AgreementText ?? "" : null,
                        HasCheckBox = d.HasCheckBox > 0 ? true : null,
                        HasRadioBox = d.HasRadioBox > 0 ? true : null,
                        HasInt = d.HasInt > 0 ? true : null,
                        HasDecimal = d.HasDecimal > 0 ? true : null,
                        HasDateTime = d.HasDateTime > 0 ? true : null,
                        HasAttachment = d.HasAttachment > 0 ? true : null,
                        Attachments = d.HasAttachment > 0 ? _mapper.Map<List<AttachmentDto>>(
                            await _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_DUE.ToString(), d.DesignId)) : null,
                        HasBankList = d.HasBankList > 0 ? true : null,
                        HasTexArea = d.HasTexArea > 0 ? true : null,
                        ParentCompanies = d.ParentCompanies,
                        HasDataGrid = d.HasGrid > 0 ? true : null,
                        GridRowLimit = d.GridRowLimit > 0 ? d.GridRowLimit : null,
                        GridColumnCount = d.GridColumnCount > 0 ? d.GridColumnCount : null,
                        HasAgreement = d.HasAgreement ? true : null,
                        GridColumns = d.HasGrid > 0 ? new[]
                        {
                           d.Column1Alias,
                           d.Column2Alias,
                           d.Column3Alias,
                           d.Column4Alias,
                           d.Column5Alias,
                        }.Where(col => col != null).ToArray() : null,
                        Scoring = correspondingValue?.Scoring
                    };

                    dto.Childs.Add(childDto);
                }
                responseModel.Add(dto);
            }

            return await SetGridDatasAsync(responseModel);
        }
        private async Task<List<DueDiligenceDesignDto>> SetGridDatasAsync(List<DueDiligenceDesignDto> dueDesign)
        {
            for (int i = 0; i < dueDesign.Count; i++)
            {
                for (int j = 0; j < dueDesign[i].Childs.Count; j++)
                {
                    if (Convert.ToBoolean(dueDesign[i].Childs[j].HasDataGrid))
                    {
                        int childDesignId = dueDesign[i].Childs[j].DesignId;
                        var gridDatas = await _repository.GetDueDiligenceGridAsync(childDesignId);
                        dueDesign[i].Childs[j].GridDatas = gridDatas.Count > 0 ? gridDatas : null;
                    }
                }
            }
            return dueDesign;
        }
    }
}

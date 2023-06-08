using AutoMapper;
using Microsoft.Extensions.Options;
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
                DueDiligenceDesign = await GetDueDesignsAsync(model.Language)
            };

            return ApiResponse<VM_GET_SupplierEvaluation>.Success(viewModel, 200);
        }

        public async Task<ApiResponse<BankCodesDto>> GetBankCodesAsync(int vendorId)
        {
            BankCodesDto bankCodes = new()
            {
                Currencies = await _repository.GetCurrenciesAsync(),
                BankDetails = await _repository.GetVondorBankDetailsAsync(vendorId),
            };

            return ApiResponse<BankCodesDto>.Success(bankCodes, 200);
        }

        public async Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity)
        {
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            VM_GET_VendorBankDetails bankDetails = new()
            {
                Currencies = await _repository.GetCurrenciesAsync(),
                BankDetails = _mapper.Map<List<VendorBankDetailDto>>(await _repository.GetVondorBankDetailsAsync(user.VendorId))
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

        public async Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(string acceptLanguage)
             => ApiResponse<List<DueDiligenceDesignDto>>.Success(await GetDueDesignsAsync(Language.en));



        public async Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity)
        {
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            var vendorPrequalificationTask = _repository.GetVendorPrequalificationAsync(user.VendorId);
            var prequalificationTypesTask = _repository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _repository.GetBusinessCategoriesAsync();
            var vendorBusinessCategoriesTask = _repository.GetVendorBuCategoriesAsync(user.VendorId);
            var companyInfoTask = _repository.GetCompanyInfoAsync(user.VendorId);
            var attachmentTask = _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_LOGO.ToString());

            await Task.WhenAll(vendorPrequalificationTask, prequalificationTypesTask, businessCategoriesTask, vendorBusinessCategoriesTask, attachmentTask, companyInfoTask);

            var matchedPrequalificationTypes = prequalificationTypesTask.Result
                .Where(x => vendorPrequalificationTask.Result.Select(y => y.PrequalificationCategoryId).Contains(x.Id))
                .ToList();

            var matchedBuCategories = businessCategoriesTask.Result
                .Where(x => vendorBusinessCategoriesTask.Result.Select(y => y.VendorBusinessCategoryId).Contains(x.Id))
                .ToList();

            CompanyInfoDto companyInfo = _mapper.Map<CompanyInfoDto>(companyInfoTask.Result);
            companyInfo.PrequalificationCategories = matchedPrequalificationTypes;
            companyInfo.BusinessCategories = matchedBuCategories;

            VM_GET_InitalRegistration viewModel = new()
            {
                CompanyInformation = companyInfo,
                BusinessCategories = businessCategoriesTask.Result,
                PaymentTerms = await _repository.GetPaymentTermsAsync(),
                PrequalificationTypes = prequalificationTypesTask.Result,
                Services = await _repository.GetProductServicesAsync(),
                ContactPerson = _mapper.Map<ContactPersonDto>(user),
                Attachments = _mapper.Map<List<AttachmentDto>>(attachmentTask.Result)
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

        private async Task<List<DueDiligenceDesignDto>> GetDueDesignsAsync(Language language)
        {
            List<DueDiligenceDesign> dueDiligence = await _repository.GetDueDiligencesDesignAsync(language);

            var tesMapping = dueDiligence
                .GroupBy(x => x.Title).ToList()
                .Select(x => new DueDiligenceDesignDto
                {
                    Title = x.Key,
                    Childs = x.Select(d => new DueDiligenceChildDto
                    {
                        DesignId = d.DesignId,
                        LineNo = d.LineNo,
                        Question = d.Question,
                        HasTextBox = d.HasTextBox > 0,
                        HasCheckBox = d.HasCheckBox > 0,
                        HasRadioBox = d.HasRadioBox > 0,
                        HasInt = d.HasInt > 0,
                        HasDecimal = d.HasDecimal > 0,
                        HasDateTime = d.HasDateTime > 0,
                        HasAttachment = d.HasAttachment > 0,
                        HasBankList = d.HasBankList > 0,
                        HasTexArea = d.HasTexArea > 0,
                        ParentCompanies = d.ParentCompanies,
                        HasDataGrid = d.HasGrid > 0,
                        GridRowLimit = d.GridRowLimit,
                        GridColumnCount = d.GridColumnCount,
                        HasAgreement = d.HasAgreement,
                        AgreementText = d.AgreementText,
                        GridColumns = new[]
                        {
                           d.Column1Alias,
                           d.Column2Alias,
                           d.Column3Alias,
                           d.Column4Alias,
                           d.Column5Alias,
                        }.Where(col => col != null).ToArray(),
                        GridDatas = new(),
                        TextBoxPoint = d.HasTextBox,
                        CheckBoxPoint = d.HasCheckBox,
                        RadioBoxPoint = d.HasRadioBox,
                        IntPoint = d.HasInt,
                        DateTimePoint = d.HasDateTime,
                        AttachmentPoint = d.HasAttachment,
                        TextAreaPoint = d.HasTexArea,
                        BankListPoint = d.HasBankList,
                        DataGridPoint = d.HasGrid,
                    }).ToList()
                }).ToList();


            return await SetGridDatasAsync(tesMapping);
        }
        private async Task<List<DueDiligenceDesignDto>> SetGridDatasAsync(List<DueDiligenceDesignDto> dueDesign)
        {
            for (int i = 0; i < dueDesign.Count; i++)
            {
                for (int j = 0; j < dueDesign[i].Childs.Count; j++)
                {
                    if (dueDesign[i].Childs[j].HasDataGrid)
                    {
                        int childDesignId = dueDesign[i].Childs[j].DesignId;
                        dueDesign[i].Childs[j].GridDatas = await _repository.GetDueDiligenceGridAsync(childDesignId);
                    }
                }
            }
            return dueDesign;
        }
    }
}

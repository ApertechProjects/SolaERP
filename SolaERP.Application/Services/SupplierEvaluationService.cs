using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class SupplierEvaluationService : ISupplierEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _buRepository;
        private readonly ISupplierEvaluationRepository _repository;

        public SupplierEvaluationService(ISupplierEvaluationRepository repository, IUnitOfWork unitOfWork, IBusinessUnitRepository buRepository, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _buRepository = buRepository;
            _mapper = mapper;
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
        public async Task<List<DueDiligenceDesignDto>> GetDueDiligenceAsync(Language language)
        {
            return await GetDueDesignsAsync(Language.en);
        }

        public async Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync()
        {
            CompanyInformation companyInformation = new()
            {
                BusinessCategories = await _repository.GetBusinessCategoriesAsync(),
                PaymentTerms = await _repository.GetPaymentTermsAsync(),
                Countries = await _repository.GetCountriesAsync(),
                PrequalificationTypes = await _repository.GetPrequalificationCategoriesAsync(),
                Services = await _repository.GetProductServicesAsync(),
            };

            VM_GET_InitalRegistration viewModel = new()
            {
                CompanyInformation = new()
                {
                    BusinessCategories = await _repository.GetBusinessCategoriesAsync(),
                    PaymentTerms = await _repository.GetPaymentTermsAsync(),
                    Countries = await _repository.GetCountriesAsync(),
                    PrequalificationTypes = await _repository.GetPrequalificationCategoriesAsync(),
                    Services = await _repository.GetProductServicesAsync(),
                },

                ContactPerson = new()
            };

            return ApiResponse<VM_GET_InitalRegistration>.Success(viewModel, 200);
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
                        dueDesign[i].Childs[j].GridDatas = await _repository.GetDueDiligenceGridsAsync(childDesignId);
                    }
                }
            }
            return dueDesign;
        }
    }
}

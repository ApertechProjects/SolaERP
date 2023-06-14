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
            await _repository.VendorRepresentedCompanyAddAsync(new Application.Models.VendorRepresentedCompany { VendorId = vendorId, RepresentedCompanyName = command.CompanyInfo.RepresentedCompanies });
            await _repository.VendorRepresentedProductAddAsync(new Application.Models.RepresentedProductData { VendorId = vendorId, RepresentedProductName = command.CompanyInfo.RepresentedCompanies });

            var companyLogo = _mapper.Map<AttachmentSaveModel>(command.CompanyInfo.CompanyLogo);
            companyLogo.SourceId = vendorId;
            companyLogo.SourceType = SourceType.VEN_LOGO.ToString();
            await _attachmentRepository.SaveAttachmentAsync(companyLogo);

            var attachments = _mapper.Map<List<AttachmentSaveModel>>(command.CompanyInfo.Attachments);
            attachments.ForEach(attachment =>
            {
                attachment.SourceId = vendorId;
                attachment.SourceType = SourceType.VEN_OLET.ToString();
            });

            for (int i = 0; i < attachments.Count; i++)
            {
                await _attachmentRepository.SaveAttachmentAsync(attachments[i]);
            }

            command.CodeOfBuConduct.ForEach(x => x.VendorId = vendorId);
            command.NonDisclosureAgreement.ForEach(x => x.VendorId = vendorId);
            command.BankAccounts.ForEach(x => x.BankDetails.VendorId = vendorId);


            List<Task<bool>> tasks = new();
            tasks.AddRange(command.BankAccounts.Select(x =>
            {
                x.BankDetails.VendorId = vendorId;

                if (x.AccountVerificationLetter is not null)
                {
                    var entity = _mapper.Map<AttachmentSaveModel>(x.AccountVerificationLetter);
                    entity.SourceId = vendorId;
                    entity.SourceType = SourceType.VEN_BNK.ToString();

                    return _attachmentRepository.SaveAttachmentAsync(entity);
                }

                return _vendorRepository.AddBankDetailsAsync(user.Id, _mapper.Map<VendorBankDetail>(x.BankDetails));
            }));


            tasks = tasks.Concat(command.DueDiligence.SelectMany(item =>
            {
                var dueInputModel = _mapper.Map<VendorDueDiligenceModel>(item);
                dueInputModel.VendorId = vendorId;

                var itemTasks = new List<Task<bool>>
                {
                      _repository.AddDueAsync(dueInputModel)
                };

                if (item.HasDataGrid == true)
                {
                    itemTasks.AddRange(item.GridDatas.Select(gridData =>
                        _repository.AddDueDesignGrid(_mapper.Map<DueDiligenceGridModel>(gridData))));
                }

                if (item.Attachments is not null)
                {
                    itemTasks.AddRange(item.Attachments.Select(attachment =>
                        _attachmentRepository.SaveAttachmentAsync(_mapper.Map<AttachmentSaveModel>(attachment))));
                }

                return itemTasks;
            })).ToList();

            tasks.AddRange(command.Prequalification.SelectMany(item =>
            {
                var prequalificationValue = _mapper.Map<VendorPrequalificationValues>(item);
                prequalificationValue.VendorId = vendorId;

                var tasksList = new List<Task<bool>>
                {
                     _repository.AddPrequalification(prequalificationValue) //+
                };

                if (item.Attachments is not null)
                {
                    tasksList.AddRange(item.Attachments.Select(attachment =>
                        _attachmentRepository.SaveAttachmentAsync(_mapper.Map<AttachmentSaveModel>(attachment))));
                }

                if (item.HasGrid == true)
                {
                    tasksList.AddRange(item.GridDatas.Select(gridData =>
                        _repository.AddPreGridAsync(_mapper.Map<Application.Entities.SupplierEvaluation.PrequalificationGridData>(gridData))));
                }

                return tasksList;
            }));


            command.NonDisclosureAgreement.ForEach(x => x.VendorId = vendorId);
            tasks.AddRange(command.NonDisclosureAgreement.Select(x => _repository.AddNDAAsync(_mapper.Map<VendorNDA>(x))));


            command.CodeOfBuConduct.ForEach(x => x.VendorId = vendorId);
            tasks.AddRange(command.CodeOfBuConduct.Select(x => _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(x))));

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
            var venLogoAttachmentTask = _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_LOGO.ToString());
            var venOletAttachmentTask = _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_OLET.ToString());
            var productServicesTask = _repository.GetProductServicesAsync();

            await Task.WhenAll(vendorPrequalificationTask,
                                prequalificationTypesTask,
                                businessCategoriesTask,
                                vendorBusinessCategoriesTask,
                                productServicesTask,
                                vendorProductsTask,
                                venOletAttachmentTask,
                                venLogoAttachmentTask,
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
            companyInfo.CompanyLogo = _mapper.Map<AttachmentDto>(venLogoAttachmentTask.Result[0]);
            companyInfo.Attachments = _mapper.Map<List<AttachmentDto>>(venOletAttachmentTask.Result);
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



        public async Task<ApiResponse<List<PrequalificationWithCategoryDto>>> GetPrequalificationAsync(string userIdentity, List<int> categoryIds, string acceptLang)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<VendorPrequalificationValues> prequalificationValues = await _repository.GetPrequalificationValuesAsync(user.VendorId);

            var responseModel = new List<PrequalificationWithCategoryDto>();
            foreach (var categoryId in categoryIds)
            {
                List<PrequalificationDesign> prequalificationDesigns = await _repository.GetPrequalificationDesignsAsync(categoryId, Language.en);
                var category = await _repository.GetPrequalificationCategoriesAsync();
                var matchedCategory = category.FirstOrDefault(x => x.Id == categoryId);

                var categoryDto = new PrequalificationWithCategoryDto
                {
                    Id = categoryId,
                    Name = matchedCategory?.Category ?? "Unknown",
                    Prequalifications = new List<VM_GET_Prequalification>()
                };

                var titleGroups = prequalificationDesigns.GroupBy(x => x.Title);
                foreach (var titleGroup in titleGroups)
                {
                    var prequalificationTasks = titleGroup.Select(async design =>
                    {
                        var correspondingValue = prequalificationValues.FirstOrDefault(v => v.PrequalificationDesignId == design.PrequalificationDesignId);

                        return new PrequalificationDto
                        {
                            DesignId = design.PrequalificationDesignId,
                            LineNo = design.LineNo,
                            Discipline = design.Discipline,
                            Questions = design.Questions,
                            HasTextbox = design.HasTextbox > 0 ? true : null,
                            HasTextarea = design.HasTextarea > 0 ? true : null,
                            HasCheckbox = design.HasCheckbox > 0 ? true : null,
                            HasRadiobox = design.HasRadiobox > 0 ? true : null,
                            HasInt = design.HasInt > 0 ? true : null,
                            HasDecimal = design.HasDecimal > 0 ? true : null,
                            HasDateTime = design.HasDateTime > 0 ? true : null,
                            HasAttachment = design.HasAttachment > 0 ? true : null,
                            Title = design.Title,
                            Weight = design.Weight,
                            HasGrid = design.HasGrid > 0 ? true : null,
                            GridRowLimit = design.HasGrid > 0 ? design.GridRowLimit : null,
                            GridColumnCount = design.HasGrid > 0 ? design.GridColumnCount : null,
                            GridColumns = design.HasGrid > 0 ? new[]
                            {
                                 design.Column1Alias,
                                 design.Column2Alias,
                                 design.Column3Alias,
                                 design.Column4Alias,
                                 design.Column5Alias,
                            }.Where(col => col != null).ToArray() : null,
                            TextboxPoint = design.HasTextbox > 0 ? design.HasTextbox : null,
                            TextareaPoint = design.HasTextarea > 0 ? design.HasTextarea : null,
                            CheckboxPoint = design.HasCheckbox > 0 ? design.HasCheckbox : null,
                            RadioboxPoint = design.HasRadiobox > 0 ? design.HasRadiobox : null,
                            IntPoint = design.HasInt > 0 ? design.HasInt : null,
                            DecimalPoint = design.HasDecimal > 0 ? design.HasDecimal : null,
                            DateTimePoint = design.HasDateTime > 0 ? design.HasDateTime : null,
                            Attachmentpoint = design.HasAttachment > 0 ? design.HasAttachment : null,
                            TextboxValue = design.HasTextbox > 0 ? correspondingValue?.TextboxValue ?? "" : null,
                            TextareaValue = design.HasTextarea > 0 ? correspondingValue?.TextareaValue ?? "" : null,
                            CheckboxValue = design.HasCheckbox > 0 ? correspondingValue?.CheckboxValue : null,
                            RadioboxValue = design.HasRadiobox > 0 ? correspondingValue?.RadioboxValue : null,
                            IntValue = design.HasInt > 0 ? correspondingValue?.IntValue : null,
                            DecimalValue = design.HasDecimal > 0 ? correspondingValue?.DecimalValue : null,
                            DateTimeValue = design.HasDateTime > 0 ? correspondingValue?.DateTimeValue : null,
                            Attachments = design.HasAttachment > 0 ? _mapper.Map<List<AttachmentDto>>(
                                await _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_PREQ.ToString(), design.PrequalificationDesignId)) : null
                        };
                    });

                    var prequalificationDtos = await Task.WhenAll(prequalificationTasks);
                    var prequalificationDto = new VM_GET_Prequalification
                    {
                        Title = titleGroup.Key,
                        Childs = prequalificationDtos.ToList()
                    };

                    categoryDto.Prequalifications.Add(prequalificationDto);
                }
                responseModel.Add(categoryDto);
            }
            var response = await SetGridDatasAsync(responseModel);
            return ApiResponse<List<PrequalificationWithCategoryDto>>.Success(response, 200);
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
                        HasTextBox = d.HasTextBox > 0,
                        TextboxValue = correspondingValue?.TextboxValue,
                        CheckboxValue = d.HasCheckBox > 0,// ? correspondingValue?.CheckboxValue : null,
                        RadioboxValue = correspondingValue?.RadioboxValue == true,
                        IntValue = Convert.ToInt32(correspondingValue?.IntValue),
                        DecimalValue = Convert.ToDecimal(correspondingValue?.DecimalValue),
                        DateTimeValue = Convert.ToDateTime(correspondingValue?.DateTimeValue),
                        TextareaValue = correspondingValue?.TextareaValue ?? "",
                        AgreementValue = correspondingValue?.AgreementValue == true,
                        AgreementText = d.HasAgreement ? d.AgreementText ?? "" : null,
                        HasCheckBox = d.HasCheckBox > 0,// ? true : null,
                        HasRadioBox = d.HasRadioBox > 0,
                        HasInt = d.HasInt > 0,
                        HasDecimal = d.HasDecimal > 0,
                        HasDateTime = d.HasDateTime > 0,
                        HasAttachment = d.HasAttachment > 0,
                        Attachments = d.HasAttachment > 0 ? _mapper.Map<List<AttachmentDto>>(
                            await _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_DUE.ToString(), d.DesignId)) : null,
                        HasBankList = d.HasBankList > 0,
                        HasTexArea = d.HasTexArea > 0,
                        ParentCompanies = d.ParentCompanies,
                        HasDataGrid = d.HasGrid > 0,
                        GridRowLimit = d.GridRowLimit, //> 0 ? d.GridRowLimit : null,
                        GridColumnCount = d.GridColumnCount, //> 0 ? d.GridColumnCount : null,
                        HasAgreement = d.HasAgreement,
                        GridColumns = /*d.HasGrid > 0 ?*/ new[]
                        {
                           d.Column1Alias,
                           d.Column2Alias,
                           d.Column3Alias,
                           d.Column4Alias,
                           d.Column5Alias,
                        }.Where(col => col != null).ToArray(),//: null,
                        TextBoxPoint = d.HasTextBox,//> 0 ? d.HasTextBox : null,
                        TextAreaPoint = d.HasTexArea,//> 0 ? d.HasTexArea : null,
                        CheckBoxPoint = d.HasCheckBox,//> 0 ? d.HasCheckBox : null,
                        RadioBoxPoint = d.HasRadioBox,//> 0 ? d.HasRadioBox : null,
                        BankListPoint = d.HasBankList,//> 0 ? d.HasBankList : null,
                        IntPoint = d.HasInt,//> 0 ? d.HasInt : null,
                        DecimalPoint = d.HasDecimal,//> 0 ? d.HasDecimal : null,
                        DateTimePoint = d.HasDateTime,//> 0 ? d.HasDateTime : null,
                        AttachmentPoint = d.HasAttachment,//> 0 ? d.HasAttachment : null,
                        Scoring = correspondingValue?.Scoring ?? 0,
                        DataGridPoint = d.HasGrid
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

        private async Task<List<PrequalificationWithCategoryDto>> SetGridDatasAsync(List<PrequalificationWithCategoryDto> preualification)
        {
            var allGridData = await _repository.GetPrequalificationGridAsync(0);
            var mappedGridData = _mapper.Map<List<Application.Dtos.SupplierEvaluation.PrequalificationGridData>>(allGridData);

            Parallel.For(0, preualification.Count, i =>
            {
                Parallel.For(0, preualification[Convert.ToInt32(i)].Prequalifications.Count, j =>
                {
                    Parallel.For(0, preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs.Count, k =>
                    {
                        if (preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs[Convert.ToInt32(k)].HasGrid != null)
                        {
                            int childDesignId = preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs[k].DesignId;
                            var gridDatas = mappedGridData.Where(x => x.DesignId == childDesignId).ToList();
                            preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs[Convert.ToInt32(k)].GridDatas = gridDatas;
                        }
                    });
                });
            });

            return preualification;
        }

    }
}

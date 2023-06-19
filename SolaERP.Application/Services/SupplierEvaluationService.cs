using AutoMapper;
using Microsoft.AspNetCore.Html;
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
using SolaERP.Application.Extensions;
using SolaERP.Application.Models;
using SolaERP.Application.Shared;
using SolaERP.Application.UnitOfWork;
using SolaERP.Infrastructure.ViewModels;
using PrequalificationGridData = SolaERP.Application.Entities.SupplierEvaluation.PrequalificationGridData;

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
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        public SupplierEvaluationService(ISupplierEvaluationRepository repository,
                                         IMapper mapper,
                                         IUnitOfWork unitOfWork,
                                         IBusinessUnitRepository buRepository,
                                         IUserRepository userRepository,
                                         IVendorRepository vendorRepository,
                                         IAttachmentRepository attachmentRepository,
                                         IStorage storage,
                                         IOptions<StorageOption> storageOption,
                                         IEmailNotificationService emailNotificationService,
                                         IMailService mailService,
                                         IUserService userService)
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
            _emailNotificationService = emailNotificationService;
            _mailService = mailService;
            _userService = userService;
        }

        public async Task<ApiResponse<bool>> AddAsync(string useridentity, SupplierRegisterCommand command)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(useridentity));
            Vendor vendor = _mapper.Map<Vendor>(command?.CompanyInformation);
            vendor.VendorId = user.VendorId;

            int vendorId = await _vendorRepository.UpdateVendorAsync(user.Id, vendor);

            await _repository.DeleteRepresentedCompanyAsync(vendorId);
            await _repository.DeleteRepresentedProductAsync(vendorId);

            await _repository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany { VendorId = vendorId, RepresentedCompanyName = string.Join(",", command?.CompanyInformation?.RepresentedCompanies) });

            await _repository.AddRepresentedProductAsync(new RepresentedProductData { VendorId = vendorId, RepresentedProductName = string.Join(",", command?.CompanyInformation?.RepresentedProducts) });

            var companyLogo = _mapper.Map<List<AttachmentSaveModel>>(command?.CompanyInformation?.CompanyLogo);
            companyLogo.ForEach(companyLogo =>
            {
                companyLogo.SourceId = vendorId;
                companyLogo.SourceType = SourceType.VEN_LOGO.ToString();
            });

            for (int i = 0; i < companyLogo.Count; i++)
            {
                await _attachmentRepository.SaveAttachmentAsync(companyLogo[i]);
            }

            var attachments = _mapper.Map<List<AttachmentSaveModel>>(command?.CompanyInformation?.Attachments);
            attachments.ForEach(attachment =>
            {
                attachment.SourceId = vendorId;
                attachment.SourceType = SourceType.VEN_OLET.ToString();
            });

            for (int i = 0; i < attachments.Count; i++)
            {
                await _attachmentRepository.SaveAttachmentAsync(attachments[i]);
            }

            command?.CodeOfBuConduct?.ForEach(x => x.VendorId = vendorId);
            command?.NonDisclosureAgreement?.ForEach(x => x.VendorId = vendorId);
            command?.BankAccounts?.ForEach(x => x.VendorId = vendorId);


            List<Task<bool>> tasks = new();
            foreach (var x in command.BankAccounts)
            {
                var detaildId = await _vendorRepository.UpdateBankDetailsAsync(user.Id, _mapper.Map<VendorBankDetail>(x));
                x.VendorId = vendorId;

                if (x.AccountVerificationLetter != null)
                {
                    tasks.AddRange(x.AccountVerificationLetter.Select(attachment =>
                    {
                        var entity = _mapper.Map<AttachmentSaveModel>(attachment);
                        entity.SourceId = detaildId;
                        entity.SourceType = SourceType.VEN_BNK.ToString();

                        return _attachmentRepository.SaveAttachmentAsync(entity);
                    }));
                }
            }


            tasks = tasks.Concat(command?.DueDiligence?.SelectMany(item =>
            {
                var dueInputModel = _mapper.Map<VendorDueDiligenceModel>(item);
                dueInputModel.VendorId = vendorId;

                var itemTasks = new List<Task<bool>>
                {
                      _repository.UpdateDueAsync(dueInputModel)
                };

                if (item.HasDataGrid == true)
                {
                    itemTasks.AddRange(item.GridDatas.Select(gridData =>
                        _repository.UpdateDueDesignGrid(_mapper.Map<DueDiligenceGridModel>(gridData))));
                }

                if (item.Attachments is not null)
                {
                    itemTasks.AddRange(item.Attachments.Select(attachment =>
                        _attachmentRepository.SaveAttachmentAsync(_mapper.Map<AttachmentSaveModel>(attachment))));
                }

                return itemTasks;
            })).ToList();

            tasks.AddRange(command?.Prequalification?.SelectMany(item =>
            {
                var prequalificationValue = _mapper.Map<VendorPrequalificationValues>(item);
                prequalificationValue.VendorId = vendorId;

                var tasksList = new List<Task<bool>>
                {
                     _repository.UpdatePrequalification(prequalificationValue) //+
                };

                if (item.Attachments is not null)
                {
                    tasksList.AddRange(item.Attachments.Select(attachment =>
                        _attachmentRepository.SaveAttachmentAsync(_mapper.Map<AttachmentSaveModel>(attachment))));
                }

                if (item.HasGrid == true)
                {
                    tasksList.AddRange(item.GridDatas.Select(gridData =>
                        _repository.UpdatePreGridAsync(_mapper.Map<Application.Entities.SupplierEvaluation.PrequalificationGridData>(gridData))));
                }

                return tasksList;
            }));


            command?.NonDisclosureAgreement?.ForEach(x => x.VendorId = vendorId);
            if (command?.NonDisclosureAgreement != null && command?.NonDisclosureAgreement?.Count > 0)
                tasks.AddRange(command?.NonDisclosureAgreement?.Select(x => _repository.AddNDAAsync(_mapper.Map<VendorNDA>(x))));


            command?.CodeOfBuConduct?.ForEach(x => x.VendorId = vendorId);

            if (command?.CodeOfBuConduct != null && command?.CodeOfBuConduct?.Count > 0)
                tasks.AddRange(command?.CodeOfBuConduct?.Select(x => _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(x))));

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

            await Task.WhenAll(currencyTask, bankDetailsTask);

            var bankAccount = _mapper.Map<List<VendorBankDetailDto>>(bankDetailsTask.Result);

            foreach (var item in bankAccount)
            {
                var attachment = _mapper.Map<List<AttachmentDto>>(await _attachmentRepository.GetAttachmentsAsync(item.Id, null, SourceType.VEN_BNK.ToString()));
                item.AccountVerificationLetter = attachment;
            }


            VM_GET_VendorBankDetails bankDetails = new()
            {
                Currencies = currencyTask.Result,
                BankDetails = bankAccount,
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
            companyInfo.PrequalificationTypes = matchedPrequalificationTypes;
            companyInfo.BusinessCategories = matchedBuCategories;
            companyInfo.CompanyLogo = _mapper.Map<List<AttachmentDto>>(venLogoAttachmentTask.Result);
            companyInfo.Attachments = _mapper.Map<List<AttachmentDto>>(venOletAttachmentTask.Result);
            companyInfo.Services = matchedProductServices;

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

            var gridDatas = await _repository.GetPrequalificationGridAsync(user.VendorId);

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
                        var attachments = _mapper.Map<List<AttachmentDto>>(
                        await _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_PREQ.ToString(), design.PrequalificationDesignId));

                        var correspondingValue = prequalificationValues.FirstOrDefault(v => v.PrequalificationDesignId == design.PrequalificationDesignId);
                        var calculationResult = CalculateScoring(correspondingValue, design, gridDatas, attachments?.Count > 0);

                        return new PrequalificationDto
                        {
                            DesignId = design.PrequalificationDesignId,
                            LineNo = design.LineNo,
                            Discipline = design.Discipline,
                            Questions = design.Questions,
                            HasTextbox = design.HasTextbox > 0,
                            HasTextarea = design.HasTextarea > 0,
                            HasCheckbox = design.HasCheckbox > 0,
                            HasRadiobox = design.HasRadiobox > 0,
                            HasInt = design.HasInt > 0,
                            HasDecimal = design.HasDecimal > 0,
                            HasDateTime = design.HasDateTime > 0,
                            HasAttachment = design.HasAttachment > 0,
                            Title = design.Title,
                            HasGrid = design.HasGrid > 0,
                            DataGridPoint = design.HasGrid,
                            GridRowLimit = design.GridRowLimit,
                            GridColumnCount = design.GridColumnCount,
                            GridColumns = new[]
                            {
                                 design.Column1Alias,
                                 design.Column2Alias,
                                 design.Column3Alias,
                                 design.Column4Alias,
                                 design.Column5Alias,
                            }.Where(col => col != null).ToArray(),
                            TextboxPoint = design.HasTextbox,
                            TextareaPoint = design.HasTextarea,
                            CheckboxPoint = design.HasCheckbox,
                            RadioboxPoint = design.HasRadiobox,
                            IntPoint = design.HasInt,
                            DecimalPoint = design.HasDecimal,
                            DateTimePoint = design.HasDateTime,
                            Attachmentpoint = design.HasAttachment,
                            TextboxValue = correspondingValue?.TextboxValue,
                            TextareaValue = correspondingValue?.TextareaValue,
                            CheckboxValue = Convert.ToBoolean(correspondingValue?.CheckboxValue),
                            RadioboxValue = Convert.ToBoolean(correspondingValue?.RadioboxValue),
                            IntValue = Convert.ToInt32(correspondingValue?.IntValue),
                            DecimalValue = Convert.ToDecimal(correspondingValue?.DecimalValue),
                            DateTimeValue = Convert.ToDateTime(correspondingValue?.DateTimeValue),
                            Attachments = attachments,
                            Weight = design.Weight,
                            Outcome = calculationResult.Outcome,
                            Scoring = calculationResult.Scoring,
                            AllPoint = calculationResult.AllPoint
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

        public async Task<ApiResponse<bool>> SubmitAsync(string userIdentity, SupplierRegisterCommand command)
        {
            var result = await AddAsync(userIdentity, command);
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            var submitResult = await _vendorRepository.VendorChangeStatus(user.VendorId, 1, user.Id);
            await _unitOfWork.SaveChangesAsync();

            List<Task> emails = new List<Task>();
            Language language = "en".GetLanguageEnumValue();
            var companyName = await _emailNotificationService.GetCompanyName(user.Email);
            var templateDataForRegistrationPending = await _emailNotificationService.GetEmailTemplateData(language, EmailTemplateKey.RGA);
            VM_RegistrationPending registrationPending = new VM_RegistrationPending()
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Header = templateDataForRegistrationPending.Header,
                Body = new HtmlString(string.Format(templateDataForRegistrationPending.Body, user.FullName)),
                Language = language,
                CompanyName = companyName,
            };

            Task VerEmail = _mailService.SendUsingTemplate(templateDataForRegistrationPending.Subject,
                                                           registrationPending,
                                                           registrationPending.TemplateName(),
                                                           registrationPending.ImageName(),
                                                           new List<string> { user.Email });
            emails.Add(VerEmail);

            var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.RP);
            for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
            {
                string enumElement = Enum.GetNames(typeof(Language))[i];
                var sendUsers = await _userService.GetAdminUsersAsync(1, enumElement.GetLanguageEnumValue());
                if (sendUsers.Count > 0)
                {
                    var templateData = templates[i];
                    VM_RegistrationIsPendingAdminApprove adminApprove = new VM_RegistrationIsPendingAdminApprove()
                    {
                        Body = new HtmlString(templateData.Body),
                        CompanyName = companyName,
                        Header = templateData.Header,
                        UserName = user.UserName,
                        CompanyOrVendorName = companyName,
                        Language = templateData.Language.GetLanguageEnumValue(),
                    };
                    Task RegEmail = _mailService.SendUsingTemplate(templateData.Subject, adminApprove, adminApprove.TemplateName(), adminApprove.ImageName(), sendUsers);
                    emails.Add(RegEmail);
                }
            }
            await Task.WhenAll(emails);

            if (result.Data && submitResult)
                return ApiResponse<bool>.Success(true,200);

            return ApiResponse<bool>.Fail(false, 400);
        }

        private async Task<List<DueDiligenceDesignDto>> GetDueDesignsAsync(string userIdentity, Language language)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<DueDiligenceDesign> dueDiligence = await _repository.GetDueDiligencesDesignAsync(language);
            List<DueDiligenceValue> dueDiligenceValues = await _repository.GetVendorDuesAsync(user.VendorId);

            var groupedList = dueDiligence.GroupBy(x => x.Title).ToList();
            var responseModel = new List<DueDiligenceDesignDto>();

            foreach (var group in groupedList)
            {
                var dto = new DueDiligenceDesignDto { Title = group.Key, Childs = new List<DueDiligenceChildDto>() };
                foreach (var d in group)
                {
                    var correspondingValue = dueDiligenceValues.FirstOrDefault(v => v.DueDiligenceDesignId == d.DesignId);
                    var attachments = d.HasAttachment > 0 ? _mapper.Map<List<AttachmentDto>>(
                            await _attachmentRepository.GetAttachmentsAsync(user.VendorId, null, SourceType.VEN_DUE.ToString(), d.DesignId)) : null;

                    var calculationResult = await CalculateScoring(correspondingValue, d, attachments?.Count > 0);
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
                        Attachments = attachments,
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
                        DataGridPoint = d.HasGrid,
                        Weight = d.Weight,
                        AllPoint = calculationResult.AllPoint,
                        Scoring = calculationResult.Scoring,
                        Outcome = calculationResult.Outcome,
                    };

                    dto.Childs.Add(childDto);
                }
                responseModel.Add(dto);
            }

            return await SetGridDatasAsync(responseModel);
        }

        private (decimal Scoring, decimal AllPoint, decimal Outcome) CalculateScoring(ValueEntity inputValue, PrequalificationDesign d, List<PrequalificationGridData> allDesignGrid, bool hasAttachment)
        {
            List<PrequalificationGridData> correspondingDesignGrid = null;

            if (d.HasGrid > 0)
                correspondingDesignGrid = allDesignGrid.Where(x => x.PreqqualificationDesignId == d.PrequalificationDesignId).ToList();

            decimal scoringSum = (!string.IsNullOrWhiteSpace(inputValue?.TextboxValue) ? d.HasTextbox : 0) +
                              (!string.IsNullOrWhiteSpace(inputValue?.TextareaValue) ? d.HasTextarea : 0) +
                              (inputValue?.CheckboxValue == true ? d.HasCheckbox : 0) +
                              (inputValue?.RadioboxValue == true ? d.HasRadiobox : 0) +
                              (inputValue?.IntValue > 0 ? d.HasInt : 0) +
                              (inputValue?.DecimalValue > 0 ? d.HasDecimal : 0) +
                              (inputValue?.DateTimeValue != null ? d.HasDateTime : 0) +
                              (hasAttachment ? d.HasAttachment : 0) +
                              (correspondingDesignGrid?.Count > 0 ? d.HasGrid : 0);

            decimal allPoint = d.HasAttachment +
                               d.HasCheckbox +
                               d.HasDateTime +
                               d.HasDecimal +
                               d.HasGrid +
                               d.HasInt +
                               d.HasRadiobox +
                               d.HasTextarea +
                               d.HasTextbox;

            decimal scoring = 0;

            if (allPoint > 0)
                scoring = (scoringSum / allPoint) * 100;

            decimal outcome = (scoring * d.Weight) / 100;

            return (scoring, allPoint, outcome);
        }

        private async Task<(decimal Scoring, decimal AllPoint, decimal Outcome)> CalculateScoring(ValueEntity inputValue, DueDiligenceDesign d, bool hasAttachment)
        {
            List<DueDiligenceGrid> dueGrid = null;
            if (d.HasGrid > 0)
                dueGrid = await _repository.GetDueDiligenceGridAsync(d.DesignId);

            decimal scoringSum = (!string.IsNullOrWhiteSpace(inputValue?.TextboxValue) ? d.HasTextBox : 0) +
                              (!string.IsNullOrWhiteSpace(inputValue?.TextareaValue) ? d.HasTexArea : 0) +
                              (inputValue?.CheckboxValue == true ? d.HasCheckBox : 0) +
                              (inputValue?.RadioboxValue == true ? d.HasRadioBox : 0) +
                              (inputValue?.IntValue > 0 ? d.HasInt : 0) +
                              (inputValue?.DecimalValue > 0 ? d.HasDecimal : 0) +
                              (inputValue?.DateTimeValue != null ? d.HasDateTime : 0) +
                              (hasAttachment ? d.HasAttachment : 0) +
                              (dueGrid?.Count > 0 ? d.HasGrid : 0);

            decimal allPoint = d.HasTextBox +
                               d.HasTexArea +
                               d.HasCheckBox +
                               d.HasRadioBox +
                               d.HasBankList +
                               d.HasInt +
                               d.HasDecimal +
                               d.HasDateTime +
                               d.HasAttachment +
                               d.HasGrid;


            decimal scoring = (scoringSum / allPoint) * 100;
            decimal outcome = (scoring * d.Weight) / 100;

            return (scoring, allPoint, outcome);
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

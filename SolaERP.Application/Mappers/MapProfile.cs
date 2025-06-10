using AutoMapper;
using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Account;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.BusinessCategory;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Country;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.DeliveryTerm;
using SolaERP.Application.Dtos.Email;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.GridLayout;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Dtos.Language;
using SolaERP.Application.Dtos.Layout;
using SolaERP.Application.Dtos.Location;
using SolaERP.Application.Dtos.LogInfo;
using SolaERP.Application.Dtos.Menu;
using SolaERP.Application.Dtos.Order;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.PaymentTerm;
using SolaERP.Application.Dtos.Procedure;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Score;
using SolaERP.Application.Dtos.Shipment;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Dtos.Supplier;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.TaxDto;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Dtos.UserDto;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Dtos.WithHoldingTax;
using SolaERP.Application.Entities.AccountCode;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.BidComparison;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.GridLayout;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.Layout;
using SolaERP.Application.Entities.Location;
using SolaERP.Application.Entities.LogInfo;
using SolaERP.Application.Entities.Menu;
using SolaERP.Application.Entities.Order;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Entities.Procedure;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.Supplier;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Translate;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;
using AnalysisCodes = SolaERP.Application.Entities.AnalysisCode.AnalysisCodes;
using Attachment = SolaERP.Application.Entities.Attachment.Attachment;
using AttachmentDto = SolaERP.Application.Dtos.Attachment.AttachmentDto;
using Currency = SolaERP.Application.Entities.Currency.Currency;
using UOM = SolaERP.Application.Entities.UOM.UOM;
using UOMDto = SolaERP.Application.Dtos.UOM.UOMDto;
using SolaERP.Job;
using SolaERP.Application.Entities.UserReport;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities.FixedAsset;
using VendorUserDto = SolaERP.Application.Dtos.Vendors.VendorUserDto;
using InvoiceRegisterDetails = SolaERP.Application.Entities.Invoice.InvoiceRegisterDetails;

namespace SolaERP.Persistence.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserDto, CurrentUserData>()
                .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserType))
                .ReverseMap();
            CreateMap<User, UserDto>().ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserTypeId))
                .ReverseMap();
            CreateMap<User, UserRegisterModel>()
                .ForMember(x => x.Position, src => src.MapFrom(z => z.Description))
                .ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsAllDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsDto>().ReverseMap();
            CreateMap<BaseBusinessUnit, BaseBusinessUnitDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitConnectionDto>().ReverseMap();
            CreateMap<Groups, GroupsDto>().ReverseMap();
            CreateMap<GroupUser, GroupUserDto>().ReverseMap();
            CreateMap<MenuWithPrivilagesDto, MenuWithPrivilages>().ReverseMap();
            CreateMap<ApprovalStagesMain, ApprovalStagesMainDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApproveStageMainId))
                .ReverseMap();
            CreateMap<ApprovalStagesMain, ApprovalStageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApproveStageMainId))
                .ReverseMap();
            CreateMap<ApprovalStagesDetail, ApprovalStageDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApproveStageDetailsId))
                .ReverseMap();
            CreateMap<ApprovalStagesDetail, ApprovalStagesDetailDto>().ReverseMap();
            CreateMap<Procedure, ProcedureDto>().ReverseMap();
            CreateMap<ApprovalStageRole, ApprovalStageRoleDto>().ReverseMap();
            //CreateMap<ApproveStageRole, ApprovalStageRoleDto>().ReverseMap();
            CreateMap<Role, ApproveRoleDto>().ReverseMap();
            CreateMap<ApproveRole, ApproveRoleDto>().ReverseMap();
            CreateMap<BusinessUnitForGroup, BusinessUnitForGroupDto>().ReverseMap();
            CreateMap<GroupMenu, GroupMenuDto>().ReverseMap();
            CreateMap<MenuWithPrivilages, GroupMenuWithPrivillageIdListDto>();
            CreateMap<RequestMain, RequestMainDto>().ReverseMap();
            CreateMap<RequestMain, RequestWFADto>().ReverseMap();
            CreateMap<RequestMain, RequestMainWithDetailsDto>().ReverseMap();
            CreateMap<RequestMain, RequestAmendmentDto>().ReverseMap();
            CreateMap<RequestAmendment, RequestAmendmentDto>().ReverseMap();
            CreateMap<RequestDetail, RequestDetailDto>().ReverseMap();
            CreateMap<RequestDetail, RequestDetailForFromStockDto>().ReverseMap();
            CreateMap<RequestCardAnalysis, RequestCardAnalysisDto>().ReverseMap();
            CreateMap<RequestTypes, RequestTypesDto>().ReverseMap();
            CreateMap<RequestMainDraftDto, RequestMainDraft>().ReverseMap();
            CreateMap<LogInfo, LogInfoDto>().ReverseMap();
            CreateMap<ItemCode, ItemCodeDto>().ReverseMap();
            CreateMap<ItemCodeInfo, ItemCodeInfoDto>().ReverseMap();
            CreateMap<RequestFollow, RequestFollowDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Buyer, BuyerDto>().ReverseMap();
            CreateMap<ActiveUser, ActiveUserDto>().ReverseMap();
            CreateMap<AnalysisCodes, AnalysisCodesDto>().ReverseMap();
            CreateMap<RequestApprovalInfo, RequestApprovalInfoDto>().ReverseMap();
            CreateMap<RequestDetail, RequestDetailsWithAnalysisCodeDto>().ReverseMap();
            CreateMap<ItemCodeWithImages, ItemCodeWithImagesDto>().ReverseMap();
            CreateMap<ApprovalStatusDto, ApprovalStatus>().ReverseMap();
            CreateMap<RequestCardMain, RequestCardMainDto>().ReverseMap();
            CreateMap<RequestCardDetail, RequestCardDetailDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<AccountCode, AccountCodeDto>().ReverseMap();
            CreateMap<RequestSaveModel, RequestMainSaveModel>().ReverseMap();

            CreateMap<Application.Entities.Currency.Currency, Application.Dtos.Currency.CurrencyDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrCode))
                .ReverseMap();

            CreateMap<Attachment, AttachmentWithFileDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.UploadDateTime))
                .ReverseMap();
            CreateMap<Attachment, AttachmentDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.UploadDateTime))
                .ReverseMap();
            CreateMap<UOM, UOMDto>().ReverseMap();
            CreateMap<AdditionalPrivilegeAccess, AdditionalPrivilegeAccessDto>().ReverseMap();
            CreateMap<RequestDetailApprovalInfo, RequestDetailApprovalInfoDto>()
                .ForMember(dest => dest.ApproveDate, opt => opt.MapFrom(src => src.ApproveDate))
                .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ApprovalStatusName, opt => opt.MapFrom(src => src.ApprovalStatusName))
                .ForMember(dest => dest.ApproveStageDetailsName, opt => opt.MapFrom(src => src.ApproveStageDetailsName))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.Sequence, opt => opt.MapFrom(src => src.Sequence)).ReverseMap();

            CreateMap<SupplierCode, SupplierCodeDto>()
                .ForMember(dest => dest.SupplierCode, opt => opt.MapFrom(src => src.SuppCode))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();

            CreateMap<Layout, LayoutDto>().ForMember(dest => dest.TabIndex, opt => opt.MapFrom(src => src.TabIndex))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.UserLayout)).ReverseMap();

            CreateMap<RequestMainDto, RequestMainAll>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Translate, TranslateDto>().ReverseMap();
            CreateMap<VendorInfo, VendorInfoDto>().ReverseMap();
            CreateMap<UserMain, UserMainDto>()
                .ForMember(dest => dest.UserPhoto, opt => opt.MapFrom(src => src.UserPhoto)).ReverseMap();
            CreateMap<UserLoad, UserLoadDto>()
                .ForMember(dest => dest.ApproveStatusId, expression => expression.MapFrom(opt => opt.ApproveStatus))
                .ReverseMap();
            CreateMap<ERPUser, ERPUserDto>().ReverseMap();
            CreateMap<AnalysisDimension, AnalysisDimensionDto>().ReverseMap();
            CreateMap<GroupAnalysisCode, GroupAnalysisCodeDto>().ReverseMap();
            CreateMap<GroupAdditionalPrivilege, GroupAdditionalPrivilegeDto>().ReverseMap();
            CreateMap<GroupBuyer, GroupBuyerDto>().ReverseMap();
            CreateMap<GroupRole, GroupRoleDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<UserSaveModel, User>().ReverseMap();
            CreateMap<UsersByGroup, UsersByGroupDto>().ReverseMap();
            CreateMap<EmailTemplateData, EmailTemplateDataDto>();
            CreateMap<Analysis, AnalysisDto>().ReverseMap();
            CreateMap<AnalysisWithBu, AnalysisWithBuDto>().ReverseMap();
            CreateMap<BuAnalysisDimension, BuAnalysisDimensionDto>().ReverseMap();
            CreateMap<AnalysisStructure, AnalysisStructureDto>().ReverseMap();
            CreateMap<PaymentTerms, PaymentTermDto>().ReverseMap();
            CreateMap<DeliveryTerms, DeliveryTermDto>().ReverseMap();
            CreateMap<VendorUser, VendorUserDto>().ReverseMap();

            CreateMap<DueDiligenceDesign, DueDiligenceChildDto>()
                .ForMember(dest => dest.TextBoxPoint, opt => opt.MapFrom(src => src.HasTextBox))
                .ForMember(dest => dest.CheckBoxPoint, opt => opt.MapFrom(src => src.HasCheckBox))
                .ForMember(dest => dest.RadioBoxPoint, opt => opt.MapFrom(src => src.HasRadioBox))
                .ForMember(dest => dest.IntPoint, opt => opt.MapFrom(src => src.HasInt))
                .ForMember(dest => dest.DateTimePoint, opt => opt.MapFrom(src => src.HasDateTime))
                .ForMember(dest => dest.AttachmentPoint, opt => opt.MapFrom(src => src.HasAttachment))
                .ForMember(dest => dest.TextAreaPoint, opt => opt.MapFrom(src => src.HasTexArea))
                .ForMember(dest => dest.BankListPoint, opt => opt.MapFrom(src => src.HasBankList))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.DataGridPoint, opt => opt.MapFrom(src => src.HasGrid)).ReverseMap();

            CreateMap<ContactPersonDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNo))
                .ReverseMap();


            CreateMap<CompanyInfo, CompanyInfoDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                //.ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.VendorBusinessCategoryId))
                //.ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.Ignore())
                .ForMember(dest => dest.RepresentedProducts, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CompanyInfo, CompanyInfoViewDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                //.ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.VendorBusinessCategoryId))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.Ignore())
                .ForMember(dest => dest.RepresentedProducts, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<NDADto, VendorNDA>()
                .ForMember(dest => dest.VendorNDAId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();


            CreateMap<VendorBankDetailDto, VendorBankDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<VendorBankDetailViewDto, VendorBankDetail>().ReverseMap();

            //CreateMap<PrequalificationDto, Prequalification>()
            //    .ForMember(dest => dest.VendorPrequalificationId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<CodeOfBuConduct, VendorCOBC>()
                .ForMember(dest => dest.VendorCOBCId, opt => opt.MapFrom(src => src.CobcID))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();

            CreateMap<NonDisclosureAgreement, VendorNDA>()
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorNDAId, opt => opt.MapFrom(src => src.NdaID))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();


            CreateMap<CompanyInfoDto, Vendor>()
                .ForMember(dest => dest.Buid, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.CompanyAdress))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.WebSite, opt => opt.MapFrom(src => src.WebSite))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.MapFrom(src => src.RepresentedCompanies))
                .ForMember(dest => dest.RepresentedProducts, opt => opt.MapFrom(src => src.RepresentedProducts))
                .ForMember(dest => dest.CreditDays, opt => opt.MapFrom(src => src.CreditDays))
                //.ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.OtherProducts, opt => opt.MapFrom(src => src.Other))
                .ForMember(dest => dest.AgreeWithDefaultDays, opt => opt.MapFrom(src => src.AgreeWithDefaultDays))
                .ReverseMap();

            CreateMap<CompanyInfoViewDto, Vendor>()
                .ForMember(dest => dest.Buid, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.CompanyAdress))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.WebSite, opt => opt.MapFrom(src => src.WebSite))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.MapFrom(src => src.RepresentedCompanies))
                .ForMember(dest => dest.RepresentedProducts, opt => opt.MapFrom(src => src.RepresentedProducts))
                .ForMember(dest => dest.CreditDays, opt => opt.MapFrom(src => src.CreditDays))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.AgreeWithDefaultDays, opt => opt.MapFrom(src => src.AgreeWithDefaultDays))
                .ReverseMap();


            //.ForMember(dest => dest.PrequalificationCategoryId, opt => opt.MapFrom(src => src.PrequalificationCategoryId))
            //.ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.BusinessCategoryId)).ReverseMap();


            CreateMap<VendorProductService, VendorProductServiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VendorProductServiceId)).ReverseMap();


            CreateMap<Application.Dtos.SupplierEvaluation.PrequalificationGridData,
                    Application.Entities.SupplierEvaluation.PrequalificationGridData>()
                .ForMember(dest => dest.PreqqualificationGridDataId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PreqqualificationDesignId, opt => opt.MapFrom(src => src.DesignId))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId)).ReverseMap();


            CreateMap<DueDiligenceChildDto, VendorDueDiligenceModel>()
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.DesignId))
                .ForMember(dest => dest.IntValue, opt => opt.MapFrom(src => src.IntValue))
                .ForMember(dest => dest.TextboxValue, opt => opt.MapFrom(src => src.TextboxValue))
                .ForMember(dest => dest.TextareaValue, opt => opt.MapFrom(src => src.TextareaValue))
                .ForMember(dest => dest.CheckboxValue, opt => opt.MapFrom(src => src.CheckboxValue))
                .ForMember(dest => dest.RadioboxValue, opt => opt.MapFrom(src => src.RadioboxValue))
                .ForMember(dest => dest.DecimalValue, opt => opt.MapFrom(src => src.DecimalValue))
                .ForMember(dest => dest.DateTimeValue, opt => opt.MapFrom(src => src.DateTimeValue))
                .ReverseMap();

            CreateMap<DueDiligenceChildSaveDto, VendorDueDiligenceModel>()
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.DesignId))
                .ForMember(dest => dest.IntValue, opt => opt.MapFrom(src => src.IntValue))
                .ForMember(dest => dest.TextboxValue, opt => opt.MapFrom(src => src.TextboxValue))
                .ForMember(dest => dest.TextareaValue, opt => opt.MapFrom(src => src.TextareaValue))
                .ForMember(dest => dest.CheckboxValue, opt => opt.MapFrom(src => src.CheckboxValue))
                .ForMember(dest => dest.RadioboxValue, opt => opt.MapFrom(src => src.RadioboxValue))
                .ForMember(dest => dest.DecimalValue, opt => opt.MapFrom(src => src.DecimalValue))
                .ForMember(dest => dest.DateTimeValue, opt => opt.Ignore());

            CreateMap<VendorDueDiligenceModel, DueDiligenceChildSaveDto>()
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.DesignId))
                .ForMember(dest => dest.IntValue, opt => opt.MapFrom(src => src.IntValue))
                .ForMember(dest => dest.TextboxValue, opt => opt.MapFrom(src => src.TextboxValue))
                .ForMember(dest => dest.TextareaValue, opt => opt.MapFrom(src => src.TextareaValue))
                .ForMember(dest => dest.CheckboxValue, opt => opt.MapFrom(src => src.CheckboxValue))
                .ForMember(dest => dest.RadioboxValue, opt => opt.MapFrom(src => src.RadioboxValue))
                .ForMember(dest => dest.DecimalValue, opt => opt.MapFrom(src => src.DecimalValue))
                .ForMember(dest => dest.DateTimeValue, opt => opt.Ignore());


            CreateMap<VendorPrequalificationValues, PrequalificationDto>()
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.PrequalificationDesignId))
                .ForMember(dest => dest.IntValue, opt => opt.MapFrom(src => src.IntValue))
                .ForMember(dest => dest.TextboxValue, opt => opt.MapFrom(src => src.TextboxValue))
                .ForMember(dest => dest.TextareaValue, opt => opt.MapFrom(src => src.TextareaValue))
                .ForMember(dest => dest.CheckboxValue, opt => opt.MapFrom(src => src.CheckboxValue))
                .ForMember(dest => dest.RadioboxValue, opt => opt.MapFrom(src => src.RadioboxValue))
                .ForMember(dest => dest.DecimalValue, opt => opt.MapFrom(src => src.DecimalValue))
                .ForMember(dest => dest.DateTimeValue, opt => opt.MapFrom(src => src.DateTimeValue)).ReverseMap();

            CreateMap<VendorPrequalificationValues, PrequalificationChildSaveDto>()
                .ForMember(dest => dest.DesignId, opt => opt.MapFrom(src => src.PrequalificationDesignId))
                .ForMember(dest => dest.IntValue, opt => opt.MapFrom(src => src.IntValue))
                .ForMember(dest => dest.TextboxValue, opt => opt.MapFrom(src => src.TextboxValue))
                .ForMember(dest => dest.TextareaValue, opt => opt.MapFrom(src => src.TextareaValue))
                .ForMember(dest => dest.CheckboxValue, opt => opt.MapFrom(src => src.CheckboxValue))
                .ForMember(dest => dest.RadioboxValue, opt => opt.MapFrom(src => src.RadioboxValue))
                .ForMember(dest => dest.DecimalValue, opt => opt.MapFrom(src => src.DecimalValue))
                .ForMember(dest => dest.DateTimeValue, opt => opt.Ignore());

            CreateMap<PrequalificationChildSaveDto, VendorPrequalificationValues>()
                .ForMember(dest => dest.PrequalificationDesignId, opt => opt.MapFrom(src => src.DesignId))
                .ForMember(dest => dest.DateTimeValue, opt => opt.Ignore());


            CreateMap<AttachmentDto, AttachmentSaveModel>()
                .ForMember(dest => dest.AttachmentId, opt => opt.MapFrom(src => src.AttachmentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.SourceId))
                .ForMember(dest => dest.SourceType, opt => opt.Ignore()) // SourceType does not exist in AttachmentDto
                .ForMember(dest => dest.ExtensionType, opt => opt.MapFrom(src => src.ExtensionType))
                .ForMember(dest => dest.AttachmentTypeId, opt => opt.MapFrom(src => src.AttachmentTypeId))
                .ForMember(dest => dest.AttachmentSubTypeId, opt => opt.MapFrom(src => src.AttachmentSubTypeId));
            //.ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size)).ReverseMap();

            CreateMap<AttachmentWithFileDto, AttachmentSaveModel>().ReverseMap();

            CreateMap<DueDiligenceGridModel, DueDiligenceGrid>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DueDesignId, opt => opt.MapFrom(src => src.DueDesignId))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId)).ReverseMap();

            CreateMap<Attachment, AttachmentDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.File, opt => opt.Ignore());

            CreateMap<AttachmentDto, Attachment>()
                .ForMember(dest => dest.AttachmentId, opt => opt.MapFrom(src => src.AttachmentId))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FileData, opt => opt.Ignore());

            CreateMap<AnalysisStructureWithBu, AnalysisStructureWithBuDto>().ReverseMap();
            CreateMap<ApproveStageMainInputModel, ApprovalStagesMain>().ReverseMap();

            CreateMap<VendorLoad, VendorLoadDto>()
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.BlackList, opt => opt.MapFrom(src => src.BlackList))
                .ForMember(dest => dest.BlackListDescription, opt => opt.MapFrom(src => src.BlackListDescription))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.ReviseNo, opt => opt.MapFrom(src => src.ReviseNo))
                .ForMember(dest => dest.ReviseDate, opt => opt.MapFrom(src => src.ReviseDate))
                .ForMember(dest => dest.ApproveStatus, opt => opt.MapFrom(src => src.ApproveStatus))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.Postal_ZIP, opt => opt.MapFrom(src => src.Postal_ZIP))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DefaultCurrency, opt => opt.MapFrom(src => src.DefaultCurrency))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.Phone_Mobile, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.ShipVia, opt => opt.MapFrom(src => src.ShipmentId))
                .ForMember(dest => dest.DeliveryTerms, opt => opt.MapFrom(src => src.DeliveryTermId))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.WithHoldingTaxId, opt => opt.MapFrom(src => src.WithHoldingTaxId))
                .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.TaxesId))
                .ForMember(dest => dest.RepresentedProducts, opt => opt.MapFrom(src => src.RepresentedProducts))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.MapFrom(src => src.RepresentedCompanies))
                .ForMember(dest => dest.CreditDays, opt => opt.MapFrom(src => src.CreditDays))
                .ForMember(dest => dest._60DaysPayment, opt => opt.MapFrom(src => src._60DaysPayment))
                .ForMember(dest => dest.OtherProducts, opt => opt.MapFrom(src => src.OtherProducts))
                .ForMember(dest => dest.CompanyRegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                .ReverseMap();

            CreateMap<VendorWFA, VendorWFADto>().ReverseMap();
            CreateMap<VendorAllDto, VendorAll>().ReverseMap();

            CreateMap<VendorCardDto, Vendor>()
                .ForMember(dest => dest.RepresentedProducts, opt => opt.MapFrom(src => src.RepresentedProducts))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.MapFrom(src => src.RepresentedCompanies))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate))
                .ForMember(dest => dest.Postal, opt => opt.MapFrom(src => src.Postal_ZIP))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.Phone_Mobile))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.ShipmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.DeliveryTermId, opt => opt.Ignore())
                .ForMember(dest => dest.TaxesId, opt => opt.Ignore())
                .ForMember(dest => dest.WithHoldingTaxId, opt => opt.Ignore())
                .ForMember(dest => dest.AgreeWithDefaultDays, opt => opt.MapFrom(src => src._60DaysPayment));

            CreateMap<VendorCardDto2, Vendor>()
                .ForMember(dest => dest.RepresentedProducts, opt => opt.MapFrom(src => src.RepresentedProducts))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.RepresentedCompanies, opt => opt.MapFrom(src => src.RepresentedCompanies))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate))
                .ForMember(dest => dest.Postal, opt => opt.MapFrom(src => src.Postal_ZIP))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.Phone_Mobile))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.ShipmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => src.PaymentTerms))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.DeliveryTermId, opt => opt.Ignore())
                .ForMember(dest => dest.TaxesId, opt => opt.Ignore())
                .ForMember(dest => dest.WithHoldingTaxId, opt => opt.Ignore())
                .ForMember(dest => dest.AgreeWithDefaultDays, opt => opt.MapFrom(src => src._60DaysPayment));

            CreateMap<RfqDraft, RfqDraftDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RFQMainId)).ReverseMap();

            CreateMap<RfqAll, RfqAllDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RFQMainId)).ReverseMap();

            CreateMap<RequestForRFQ, RequestRfqDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RequestMainId)).ReverseMap();
            CreateMap<GridLayout, GridLayoutDto>().ReverseMap();
            CreateMap<Application.Entities.AnalysisCode.AnalysisCode, AnalysisCodeDto>().ReverseMap();

            CreateMap<Application.Entities.AnalysisCode.AnalysisCode, AnalysisCodeDto>().ReverseMap();
            CreateMap<RFQMain, RFQMainDto>().ReverseMap();
            CreateMap<RFQDetail, RFQDetailDto>()
                .ForMember(dest => dest.Conversion, opt => opt.MapFrom(src => src.CONV_ID)).ReverseMap();
            CreateMap<RFQRequestDetail, RFQRequestDetailDto>().ReverseMap();
            CreateMap<RejectReason, RejectReasonDto>().ReverseMap();
            CreateMap<RFQInProgress, RFQInProgressDto>().ReverseMap();
            CreateMap<RFQSingleSourceReasonsLoad, RFQSingleSourceReasonsLoadDto>().ReverseMap();

            CreateMap<BidAll, BidAllDto>().ReverseMap();

            CreateMap<BidDetailsFilter, BidDetailsFilterDto>().ReverseMap();
            CreateMap<BidDetailsLoad, BidDetailsLoadDto>().ReverseMap();

            CreateMap<BidAllFilter, BidAllFilterDto>()
                .ReverseMap()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => string.Join(",", src.Status.Select(x => (int)x))))
                .ForMember(dest => dest.ApproveStatus,
                    opt => opt.MapFrom(src => string.Join(",", src.ApproveStatus.Select(x => (int)x))))
                .ForMember(dest => dest.Emergency,
                    opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));

            CreateMap<BidAllFilter, BidDraftFilterDto>()
                .ReverseMap()
                .ForMember(dest => dest.ApproveStatus,
                    opt => opt.MapFrom(src => string.Join(",", src.ApproveStatus.Select(x => (int)x))))
                .ForMember(dest => dest.Emergency,
                    opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));

            CreateMap<BidAllFilter, BidSubmittedFilterDto>()
                .ReverseMap()
                .ForMember(dest => dest.ApproveStatus,
                    opt => opt.MapFrom(src => string.Join(",", src.ApproveStatus.Select(x => (int)x))))
                .ForMember(dest => dest.Emergency,
                    opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));

            CreateMap<BidMain, BidMainDto>().ReverseMap();
            CreateMap<BidMainLoad, BidMainLoadDto>().ReverseMap();
            CreateMap<BidDetail, BidDetailDto>().ReverseMap();
            CreateMap<BidRFQListLoad, BidRFQListLoadDto>().ReverseMap();
            CreateMap<BidDisqualify, BidDisqualifyDto>().ReverseMap();

            CreateMap<Application.Entities.RFQ.UOM, Application.Dtos.RFQ.UOMDto>()
                .ForMember(dest => dest.UOM, opt => opt.MapFrom(src => src.UnitOfMeasure)).ReverseMap();

            CreateMap<BidComparisonAllFilter, BidComparisonAllFilterDto>()
                .ReverseMap()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => string.Join(",", src.Status.Select(x => (int)x))))
                .ForMember(dest => dest.ApproveStatus,
                    opt => opt.MapFrom(src => string.Join(",", src.ApprovalStatus.Select(x => (int)x))))
                .ForMember(dest => dest.Emergency,
                    opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonAll, BidComparisonAllDto>().ReverseMap();

            CreateMap<BidComparisonIUD, BidComparisonCreateDto>().ReverseMap();
            CreateMap<BidComparisonApprove, BidComparisonApproveDto>().ReverseMap();
            CreateMap<BidComparisonSendToApprove, BidComparisonSendToApproveDto>().ReverseMap();

            CreateMap<BidComparisonBidApprovalsFilter, BidComparionBidApprovalsFilterDto>().ReverseMap();
            CreateMap<BidComparisonBidApprovalsLoad, BidComparisonBidApprovalsLoadDto>().ReverseMap();

            CreateMap<BidComparisonBidDetailsLoad, BidComparisonBidDetailsLoadDto>().ReverseMap();
            CreateMap<BidComparisonHeaderLoad, BidComparisonHeaderLoadDto>()
                .ForMember(dest => dest.ProcurementType,
                    opt => opt.MapFrom(src => ((Application.Enums.ProcurementType)src.ProcurementType).ToString()))
                .ForMember(dest => dest.Emergency,
                    opt => opt.MapFrom(src => ((Application.Enums.Emergency)src.Emergency).ToString())).ReverseMap();

            CreateMap<BidComparisonBidHeaderLoad, BidComparisonBidHeaderLoadDto>().ReverseMap();
            CreateMap<BidComparisonRFQDetailsLoad, BidComparisonRFQDetailsLoadDto>().ReverseMap();
            CreateMap<BidComparisonApprovalInformationFilter, BidComparisonApprovalInformationFilterDto>().ReverseMap();
            CreateMap<BidComparisonApprovalInformationLoad, BidComparisonApprovalInformationLoadDto>().ReverseMap();

            CreateMap<BidComparisonDraftFilter, BidComparisonDraftFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonDraftLoad, BidComparisonDraftLoadDto>().ReverseMap();


            CreateMap<BidComparisonHeldFilter, BidComparisonHeldFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonHeldLoad, BidComparisonHeldLoadDto>().ReverseMap();

            CreateMap<BidComparisonMyChartsFilter, BidComparisonMyChartsFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x)))).ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => string.Join(",", src.Status.Select(x => (int)x)))).ForMember(
                dest => dest.ApproveStatus,
                opt => opt.MapFrom(src => string.Join(",", src.ApprovalStatus.Select(x => (int)x))));
            CreateMap<BidComparisonMyChartsLoad, BidComparisonMyChartsLoadDto>().ReverseMap();

            CreateMap<BidComparisonNotReleasedFilter, BidComparisonNotReleasedFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonNotReleasedLoad, BidComparisonNotReleasedLoadDto>().ReverseMap();

            CreateMap<BidComparisonRejectedFilter, BidComparisonRejectedFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonRejectedLoad, BidComparisonRejectedLoadDto>().ReverseMap();

            CreateMap<BidComparisonWFAFilter, BidComparisonWFAFilterDto>().ReverseMap().ForMember(
                dest => dest.Emergency,
                opt => opt.MapFrom(src => string.Join(",", src.Emergency.Select(x => (int)x))));
            CreateMap<BidComparisonWFALoad, BidComparisonWFALoadDto>().ReverseMap();

            CreateMap<BidComparisonSingleSourceReasonsFilter, BidComparisonSingleSourceReasonsFilterDto>().ReverseMap();
            CreateMap<BidComparisonSingleSourceReasonsLoad, BidComparisonSingleSourceReasonsLoadDto>().ReverseMap();
            CreateMap<RFQVendorIUDDto, RFQVendorIUD>().ReverseMap();

            CreateMap<Application.Entities.User.UserList, Application.Dtos.User.UserList>().ReverseMap();


            CreateMap<RequestHeld, RequestHeldDto>().ReverseMap();

            CreateMap<CreateAdvance, CreateAdvanceDto>().ReverseMap();
            CreateMap<CreateOrder, CreateOrderDto>().ReverseMap();
            CreateMap<CreateBalance, CreateBalanceDto>().ReverseMap();
            CreateMap<InfoHeader, InfoHeaderDto>().ReverseMap();
            CreateMap<InfoDetail, InfoDetailDto>()
                .ForMember(x => x.PaymentTermsName, y => y.MapFrom(x => x.PaymentTermName))
                .ReverseMap();
            CreateMap<InfoApproval, InfoApprovalDto>().ReverseMap();

            CreateMap<WaitingForApproval, WaitingForApprovalDto>().ReverseMap();
            CreateMap<All, AllDto>().ReverseMap();
            CreateMap<Draft, DraftDto>().ReverseMap();
            CreateMap<Approved, ApprovedDto>().ReverseMap();
            CreateMap<Rejected, RejectedDto>().ReverseMap();
            CreateMap<Held, HeldDto>().ReverseMap();
            CreateMap<Bank, BankDto>().ReverseMap();
            CreateMap<SolaERP.Application.Entities.Payment.Attachment, SolaERP.Application.Dtos.Payment.AttachmentDto>()
                .ReverseMap();
            CreateMap<PaymentRequest, PaymentRequestDto>().ReverseMap();

            CreateMap<PaymentOrderMain, PaymentOrderMainDto>()
                .ForMember(x => x.JournalNumber, y => y.MapFrom(x => x.JournalNo))
                .ForMember(x => x.AllocateAmounttoPay, y => y.MapFrom(x => x.Amount))
                .ForMember(x => x.PaymentOrder, y => y.MapFrom(x => x.PaymentOrderNo))
                .ForMember(x => x.EntryDate, y => y.MapFrom(x => x.CreatedDate))
                .ReverseMap();
            CreateMap<PaymentOrderDetail, PaymentOrderDetailDto>().ReverseMap();
            CreateMap<PaymentOrderTransaction, PaymentOrderTransactionDto>().ReverseMap();
            CreateMap<BankAccountList, BankAccountListDto>().ReverseMap();
            CreateMap<ASalfldgDto, ASalfldg>()
                .ForMember(x => x.InvoiceNo, y => y.Ignore())
                .ForMember(x => x.Reference, y => y.Ignore())
                .ReverseMap();

            CreateMap<RegisterWFA, RegisterWFADto>()
                .ForMember(x => x.LinkedGRNsServices, y => y.MapFrom(x => x.GRNList))
                .ReverseMap();

            CreateMap<RegisterAll, RegisterAllDto>().ReverseMap();
            CreateMap<RegisterMainLoad, RegisterMainLoadDto>().ReverseMap();
            CreateMap<RegisterLoadGRN, RegisterLoadGRNDto>().ReverseMap();
            CreateMap<RegisterListByOrder, RegisterListByOrderDto>().ReverseMap();
            CreateMap<OrderListApproved, OrderListApprovedDto>().ReverseMap();
            CreateMap<ProblematicInvoiceReason, ProblematicInvoiceReasonDto>()
                .ForMember(x => x.ProblematicInvoiceReason, y => y.MapFrom(x => x.ProblematicInvoiceReasin))
                .ReverseMap();


            CreateMap<MatchingMainGRN, MatchingMainGRNDto>().ReverseMap();
            CreateMap<MatchingMainService, MatchingMainServiceDto>().ReverseMap();

            CreateMap<PaymentTransaction, PaymentDocumentPost>().ReverseMap();

            CreateMap<AllocationData, AllocationDataDto>().ReverseMap();
            CreateMap<PaymentOrder, PaymentOrderDto>().ReverseMap();
            CreateMap<MatchingMain, MatchingMainDto>().ReverseMap();

            CreateMap<InvoiceRegisterDetailForPO, InvoiceRegisterDetailForPODto>()
                .ForMember(x => x.Quantity, y => y.MapFrom(x => x.GRNQTY))
                .ReverseMap();

            CreateMap<InvoiceRegisterDetailForOther, InvoiceRegisterDetailForOtherDto>().ReverseMap();
            CreateMap<InvoiceRegisterDetailForGeneral, InvoiceRegisterDetailForGeneralDto>().ReverseMap();


            CreateMap<VendorApproved, VendorApprovedDto>().ReverseMap();
            CreateMap<AdvanceInvoice, AdvanceInvoiceDto>().ReverseMap();

            CreateMap<InvoiceRegisterByOrderMainId, InvoiceRegisterByOrderMainIdDto>().ReverseMap();

            CreateMap<InvoiceRegisterServiceDetailsLoad, InvoiceRegisterServiceDetailsLoadDto>().ReverseMap();

            CreateMap<BuyersAssignment, BuyersAssignmentDto>().ReverseMap();

            CreateMap<RegisterDraft, RegisterDraftDto>().ReverseMap();
            CreateMap<RegisterHeld, RegisterHeldDto>().ReverseMap();
            CreateMap<ApprovalInfo, ApprovalInfoDto>().ReverseMap();

            CreateMap<InvoiceMatchingMainGRN, InvoiceMatchingMainGRNDto>().ReverseMap();

            CreateMap<InvoicesMatchingDetailsTypeDto, InvoicesMatchingDetailsType>()
                .ForMember(x => x.AdvanceTotal, y => y.MapFrom(x => x.AdvanceAmount))
                .ForMember(x => x.Quantity, y => y.MapFrom(x => x.ServiceAmount))
                .ForMember(x => x.Total, y => y.MapFrom(x => x.InvoiceAmount))
                .ReverseMap();

            CreateMap<InvoiceMatchMainData, InvoiceMatchMainDataDto>().ReverseMap();
            CreateMap<InvoiceMatchDetailData, InvoiceMatchDetailDataDto>().ReverseMap();
            CreateMap<InvoiceMatchAdvance, InvoiceMatchAdvanceDto>().ReverseMap();
            CreateMap<InvoiceMatchGRN, InvoiceMatchGRNDto>().ReverseMap();

            CreateMap<OrderMainBaseReportInfo, OrderMainBaseReportInfoDto>().ReverseMap();
            CreateMap<Score, ScoreDto>().ReverseMap();
            CreateMap<Shipment, ShipmentDto>().ReverseMap();
            CreateMap<WithHoldingTaxData, WithHoldingTaxDto>().ReverseMap();
            CreateMap<TaxData, TaxDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<BusinessCategory, BusinessCategoryDto>()
                .ForMember(dest => dest.BusinessCategoryCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BusinessCategoryName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BusinessSectorId, opt => opt.MapFrom(src => src.BusinessSectorId))
                .ReverseMap();

            CreateMap<RowInfo, RowInfoDraft>().ReverseMap();


            CreateMap<UserReportFileAccess, UserReportFileAccessDto>().ReverseMap();

            CreateMap<VendorUserForMail, VendorUserForMailDto>().ReverseMap();

            CreateMap<VendorBusinessCategory, VendorBusinessCategoryDto>().ReverseMap();
            CreateMap<InvoiceRegisterPayablesTransactions, InvoiceRegisterPayablesTransactionsDto>().ReverseMap();

            CreateMap<InvoiceRegisterLoad, InvoiceRegisterLoadDto>()
                .ForMember(dest => dest.OrderTypeId, opt => opt.MapFrom(src => src.OrderType))
                .ReverseMap();

            CreateMap<InvoiceRegisterDetails, InvoiceRegisterDetailsDto>().ReverseMap();
            CreateMap<InvoiceRegisterGetDetails, InvoiceRegisterGetDetailsDto>().ReverseMap();
            CreateMap<VendorBaseInfo, VendorBaseInfoDto>().ReverseMap();

            CreateMap<WithHoldingTaxData, WithHoldingTaxDto>().ReverseMap();

            CreateMap<InvoiceRegisterOrderDetail, InvoiceRegisterOrderDetailDto>()
                .ForMember(dest => dest.LineDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UOM, opt => opt.MapFrom(src => src.RUOM))
                .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();

            CreateMap<RfqVendors, RFQVendorsDto>().ReverseMap();

            CreateMap<BidComparisonSummary, BidComparisonSummaryDto>().ReverseMap();

            CreateMap<FixedAsset, FixedAssetDto>()
                .ForMember(dest => dest.FixedAssetCode, opt => opt.MapFrom(src => src.AssetCode))
                .ForMember(dest => dest.FixedAssetDescription, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            CreateMap<InvoiceRegisterAdvance, InvoiceRegisterAdvanceDto>().ReverseMap();
            CreateMap<InvoiceRegisterAdvanceClosingList, InvoiceRegisterAdvanceClosingListDto>().ReverseMap();
        }
    }
}
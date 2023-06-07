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
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Email;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Dtos.Language;
using SolaERP.Application.Dtos.Layout;
using SolaERP.Application.Dtos.Location;
using SolaERP.Application.Dtos.LogInfo;
using SolaERP.Application.Dtos.Menu;
using SolaERP.Application.Dtos.Procedure;
using SolaERP.Application.Dtos.Request;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Dtos.Supplier;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Dtos.UOM;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Dtos.UserDto;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.AccountCode;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.Layout;
using SolaERP.Application.Entities.Location;
using SolaERP.Application.Entities.LogInfo;
using SolaERP.Application.Entities.Menu;
using SolaERP.Application.Entities.Procedure;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.Supplier;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Translate;
using SolaERP.Application.Entities.UOM;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;
using SolaERP.Persistence.Services;

namespace SolaERP.Persistence.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserRegisterModel>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsAllDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsDto>().ReverseMap();
            CreateMap<BaseBusinessUnit, BaseBusinessUnitDto>().ReverseMap();
            CreateMap<Groups, GroupsDto>().ReverseMap();
            CreateMap<GroupUser, GroupUserDto>().ReverseMap();
            CreateMap<MenuWithPrivilagesDto, MenuWithPrivilages>().ReverseMap();
            CreateMap<ApproveStagesMain, ApproveStagesMainDto>().ReverseMap();
            CreateMap<ApproveStagesDetail, ApproveStagesDetailDto>().ReverseMap();
            CreateMap<Procedure, ProcedureDto>().ReverseMap();
            CreateMap<ApproveStageRole, ApproveStageRoleDto>().ReverseMap();
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
            CreateMap<RequestTypes, RequestTypesDto>().ReverseMap();
            CreateMap<RequestMainDraftDto, RequestMainDraft>().ReverseMap();
            CreateMap<LogInfo, LogInfoDto>().ReverseMap();
            CreateMap<ItemCode, ItemCodeDto>().ReverseMap();
            CreateMap<ItemCodeInfo, ItemCodeInfoDto>().ReverseMap();
            CreateMap<RequestFollow, RequestFollowDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Buyer, BuyerDto>().ReverseMap();
            CreateMap<ActiveUser, ActiveUserDto>().ReverseMap();
            CreateMap<AnalysisCode, AnalysisCodeDto>().ReverseMap();
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
            CreateMap<SolaERP.Application.Entities.SupplierEvaluation.Currency, CurrencyDto>().ReverseMap();
            CreateMap<Attachment, AttachmentWithFileDto>().ReverseMap();
            CreateMap<Attachment, AttachmentDto>().ReverseMap();
            CreateMap<UOM, UOMDto>().ReverseMap();
            CreateMap<AdditionalPrivilegeAccess, AdditionalPrivilegeAccessDto>().ReverseMap();
            CreateMap<RequestDetailApprovalInfo, RequestDetailApprovalInfoDto>().
                ForMember(dest => dest.ApproveDate, opt => opt.MapFrom(src => src.ApproveDate)).
                ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.FullName)).
                ForMember(dest => dest.ApproveStatusName, opt => opt.MapFrom(src => src.ApproveStatusName)).
                ForMember(dest => dest.ApproveStageDetailsName, opt => opt.MapFrom(src => src.ApproveStageDetailsName)).
                ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment)).
                ForMember(dest => dest.Sequence, opt => opt.MapFrom(src => src.Sequence)).ReverseMap();

            CreateMap<SupplierCode, SupplierCodeDto>().ForMember(dest => dest.SupplierCode, opt => opt.MapFrom(src => src.SuppCode)).
                ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId)).
                ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();

            CreateMap<Layout, LayoutDto>().ForMember(dest => dest.TabIndex, opt => opt.MapFrom(src => src.TabIndex)).
                ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key)).
                ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.UserLayout)).ReverseMap();

            CreateMap<RequestMainDto, RequestMainAll>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Translate, TranslateDto>().ReverseMap();
            CreateMap<VendorInfo, VendorInfoDto>().ReverseMap();
            CreateMap<UserMain, UserMainDto>().ReverseMap();
            CreateMap<UserLoad, UserLoadDto>().ReverseMap();
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

            CreateMap<DueDiligenceDesign, DueDiligenceChildDto>()
                .ForMember(dest => dest.TextBoxPoint, opt => opt.MapFrom(src => src.HasTextBox))
                .ForMember(dest => dest.CheckBoxPoint, opt => opt.MapFrom(src => src.HasCheckBox))
                .ForMember(dest => dest.RadioBoxPoint, opt => opt.MapFrom(src => src.HasRadioBox))
                .ForMember(dest => dest.IntPoint, opt => opt.MapFrom(src => src.HasInt))
                .ForMember(dest => dest.DateTimePoint, opt => opt.MapFrom(src => src.HasDateTime))
                .ForMember(dest => dest.AttachmentPoint, opt => opt.MapFrom(src => src.HasAttachment))
                .ForMember(dest => dest.TextAreaPoint, opt => opt.MapFrom(src => src.HasTexArea))
                .ForMember(dest => dest.BankListPoint, opt => opt.MapFrom(src => src.HasBankList))
                .ForMember(dest => dest.DataGridPoint, opt => opt.MapFrom(src => src.HasGrid)).ReverseMap();

            CreateMap<ContactPersonDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Fullname))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();
            //TODO: Map Position also


            CreateMap<CompanyInfo, CompanyInfoDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.CompanyAdress, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.TaxOffice, opt => opt.MapFrom(src => src.TaxOffice))
                .ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.VendorBusinessCategoryId))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CompanyRegistrationDate)).ReverseMap();

            CreateMap<NDADto, VendorNDA>()
                .ForMember(dest => dest.VendorNDAId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();


            CreateMap<VendorBankDetailDto, VendorBankDetail>()
                .ForMember(dest => dest.VendorBankDetailId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<PrequalificationDto, Prequalification>()
                .ForMember(dest => dest.VendorPrequalificationId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<CodeOfBuConduct, VendorCOBC>()
                .ForMember(dest => dest.VendorCOBCId, opt => opt.MapFrom(src => src.CobcID))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();

            CreateMap<NonDisclosureAgreement, VendorNDA>()
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorNDAId, opt => opt.MapFrom(src => src.NdaID))
                .ForMember(dest => dest.BusinessUnitId, opt => opt.MapFrom(src => src.BusinessUnitId)).ReverseMap();


            CreateMap<CompanyInfoDto, Vendor>()
                 .ForMember(dest => dest.VendorId, opt => opt.Ignore())
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
                 .ForMember(dest => dest.PrequalificationCategoryId, opt => opt.MapFrom(src => src.PrequalificationCategoryId))
                 .ForMember(dest => dest.BusinessCategoryId, opt => opt.MapFrom(src => src.BusinessCategoryId)).ReverseMap();



            CreateMap<AnalysisStructureWithBu, AnalysisStructureWithBuDto>().ReverseMap();
        }
    }
}

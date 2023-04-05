using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Account;
using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Attachment;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Currency;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.Item_Code;
using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Location;
using SolaERP.Infrastructure.Dtos.LogInfo;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Procedure;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Status;
using SolaERP.Infrastructure.Dtos.Supplier;
using SolaERP.Infrastructure.Dtos.Translate;
using SolaERP.Infrastructure.Dtos.UOM;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Dtos.Venndors;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.Account;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.AnalysisDimension;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Entities.ApproveStages;
using SolaERP.Infrastructure.Entities.Attachment;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Currency;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Entities.Language;
using SolaERP.Infrastructure.Entities.Layout;
using SolaERP.Infrastructure.Entities.Location;
using SolaERP.Infrastructure.Entities.LogInfo;
using SolaERP.Infrastructure.Entities.Menu;
using SolaERP.Infrastructure.Entities.Procedure;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Entities.Status;
using SolaERP.Infrastructure.Entities.Supplier;
using SolaERP.Infrastructure.Entities.Translate;
using SolaERP.Infrastructure.Entities.UOM;
using SolaERP.Infrastructure.Entities.User;
using SolaERP.Infrastructure.Entities.Vendors;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Mappers
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
            CreateMap<MenuWithPrivilagesDto, MenuWithPrivilages>().ReverseMap();
            CreateMap<ApproveStagesMain, ApproveStagesMainDto>().ReverseMap();
            CreateMap<ApproveStagesDetail, ApproveStagesDetailDto>().ReverseMap();
            CreateMap<Procedure, ProcedureDto>().ReverseMap();
            CreateMap<ApproveStageRole, ApproveStageRoleDto>().ReverseMap();
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
            CreateMap<RequestApprovalInfo, RequestApprovalInfoDto>().ReverseMap();
            CreateMap<RequestDetail, RequestDetailsWithAnalysisCodeDto>().ReverseMap();
            CreateMap<ItemCodeWithImages, ItemCodeWithImagesDto>().ReverseMap();
            CreateMap<ApprovalStatusDto, ApprovalStatus>().ReverseMap();
            CreateMap<RequestCardMain, RequestCardMainDto>().ReverseMap();
            CreateMap<RequestCardDetail, RequestCardDetailDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<AccountCode, AccountCodeDto>().ReverseMap();
            CreateMap<RequestSaveModel, RequestMainSaveModel>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
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
        }
    }
}

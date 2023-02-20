namespace SolaERP.Application.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
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
            CreateMap<RequestMain, RequestApproveAmendmentDto>().ReverseMap();
            CreateMap<RequestDetail, RequestDetailDto>().ReverseMap();
            CreateMap<RequestTypes, RequestTypesDto>().ReverseMap();
            CreateMap<RequestMainDraftDto, RequestMainDraft>().ReverseMap();
            CreateMap<LogInfo, LogInfoDto>().ReverseMap();
            CreateMap<ItemCode, ItemCodeDto>().ReverseMap();
            CreateMap<RequestAmendment, RequestApproveAmendmentDto>().ReverseMap();
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

        }
    }
}

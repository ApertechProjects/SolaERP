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
            CreateMap<AnalysisCode, AnalysisCodeDto>().ReverseMap();
            CreateMap<RequestApprovalInfo, RequestApprovalInfoDto>().ReverseMap();
            CreateMap<RequestDetailWithAnalysisCode, RequestDetailsWithAnalysisCodeDto>().ReverseMap();
            CreateMap<ItemCodeWithImages, ItemCodeWithImagesDto>().ReverseMap();
            CreateMap<ApprovalStatusDto, ApprovalStatus>().ReverseMap();
        }
    }
}

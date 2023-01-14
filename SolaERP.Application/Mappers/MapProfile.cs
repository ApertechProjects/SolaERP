using AutoMapper;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Procedure;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.ApproveRole;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Entities.Menu;
using SolaERP.Infrastructure.Entities.Procedure;
using SolaERP.Infrastructure.Entities.Request;

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
            CreateMap<RequestMain, RequestApproveAmendmentDto>().ReverseMap();
            CreateMap<RequestTypes, RequestTypesDto>().ReverseMap();
            CreateMap<RequestMainDraftDto, RequestMainDraft>().ReverseMap();
        }
    }
}

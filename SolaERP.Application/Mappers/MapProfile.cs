using AutoMapper;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Entities.Menu;

namespace SolaERP.Application.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserUpdatePasswordDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsAllDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsDto>().ReverseMap();
            CreateMap<Groups, GroupsDto>().ReverseMap();
            CreateMap<MenuLoadDto, MenuLoad>().ReverseMap();
        }
    }
}

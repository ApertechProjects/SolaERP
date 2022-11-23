using AutoMapper;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.BusinessUnits;

namespace SolaERP.Application.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<BusinessUnits, BusinessUnitsAllDto>().ReverseMap();
        }
    }
}

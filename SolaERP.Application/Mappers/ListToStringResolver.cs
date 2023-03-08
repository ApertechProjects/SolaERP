using AutoMapper;

namespace SolaERP.Application.Mappers
{
    public class ListToStringResolver<TDestination> : IValueResolver<List<string>, TDestination, string>
    {
        public string Resolve(List<string> source, TDestination destination, string destMember, ResolutionContext context)
        {
            return string.Join(',', source);
        }
    }
}

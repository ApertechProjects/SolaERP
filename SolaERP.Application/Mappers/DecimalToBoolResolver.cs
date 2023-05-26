using AutoMapper;

namespace SolaERP.Persistence.Mappers
{
    public class DecimalToBooleanResolver : IValueResolver<object, object, bool>
    {
        public bool Resolve(object source, object destination, bool destMember, ResolutionContext context)
        {
            decimal value = (decimal)source;
            return value > 0;
        }
    }
}

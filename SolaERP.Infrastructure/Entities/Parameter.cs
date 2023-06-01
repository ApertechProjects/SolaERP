using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities
{
    public class Parameter : BaseEntity
    {
        [DbColumn("Parameter_name")]
        public string ParameterName { get; set; }
        public string Type { get; set; }
    }
}

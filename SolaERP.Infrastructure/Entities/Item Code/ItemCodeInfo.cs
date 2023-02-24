using SolaERP.Infrastructure.Attributes;
using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Dtos.Item_Code
{
    public class ItemCodeInfo : BaseEntity
    {
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string UnitOfPurch { get; set; }
        public decimal AvailableInMainStock { get; set; }
    }
}

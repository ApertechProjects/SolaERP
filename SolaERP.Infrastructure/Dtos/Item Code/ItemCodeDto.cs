using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Dtos
{
    public class ItemCodeDto : BaseEntity
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string UnitOfPurch { get; set; }
    }


}

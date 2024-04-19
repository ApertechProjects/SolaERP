using SolaERP.Application.Entities;

namespace SolaERP.Application.Dtos
{
    public class ItemCodeDto : BaseEntity
    {
        public string Item_Code { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string UnitOfPurch { get; set; }
        public string ItemDescriptionAze { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string BusinessCategoryKey { get; set; }
    }
}
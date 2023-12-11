using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.Item_Code
{
    public class ItemCode : BaseEntity
    {
        [DbColumn("ItemCode")]
        public string Item_Code { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string UnitOfPurch { get; set; }
        public string ItemDescriptionAze { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }
}

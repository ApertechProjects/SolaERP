using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Dtos.Item_Code
{
    public class ItemCodeTest
    {

        [DbColumn("ItemCodes")]
        public string Item_Code { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string UnitOfPurch { get; set; }
        public byte[] ItemImage { get; set; }
    }
}

using SolaERP.Application.Entities;

namespace SolaERP.Application.Dtos.Item_Code
{
    public class ItemCodeInfo : BaseEntity
    {
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string UnitOfPurch { get; set; }
        public decimal AvailableInMainStock { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }
}

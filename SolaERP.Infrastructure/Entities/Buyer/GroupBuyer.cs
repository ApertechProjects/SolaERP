namespace SolaERP.Application.Entities.Buyer
{
    public class GroupBuyer : BaseEntity
    {
        public int GroupBuyerId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
    }
}

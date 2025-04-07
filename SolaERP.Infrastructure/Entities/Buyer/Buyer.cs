namespace SolaERP.Application.Entities.Buyer
{
    public class Buyer : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
    }
}

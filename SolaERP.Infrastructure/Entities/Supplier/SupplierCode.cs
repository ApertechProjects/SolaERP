namespace SolaERP.Application.Entities.Supplier
{
    public class SupplierCode : BaseEntity
    {
        public int SupplierId { get; set; }
        public string SuppCode { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
    }
}

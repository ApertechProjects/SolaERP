using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class BusinessCategory : BaseEntity
    {
        [DbColumn("BusinessCategoryId")]
        public int Id { get; set; }
        [DbColumn("BusinessCategoryName")]
        public string Name { get; set; }
        public int BusinessSectorId { get; set; }
    }
}

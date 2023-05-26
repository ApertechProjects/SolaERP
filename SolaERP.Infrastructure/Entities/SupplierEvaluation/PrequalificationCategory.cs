using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class PrequalificationCategory : BaseEntity
    {
        [DbColumn("PrequalificationCategoryId")]
        public int Id { get; set; }
        [DbColumn("PrequalificationCategoryName")]
        public string Category { get; set; }
    }
}

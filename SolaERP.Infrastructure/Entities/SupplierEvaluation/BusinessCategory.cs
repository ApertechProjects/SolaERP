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
        [DbColumn("BusinessCategoryCode")]
        public string Code { get; set; }
    }


    public class BusinessCategoryForRFQ : BaseEntity
    {
        [DbColumn("BusinessCategoryId")]
        public int Id { get; set; }
        [DbColumn("BusinessCategoryName")]
        public string Name { get; set; }
        //public int BusinessSectorId { get; set; }
    }
}

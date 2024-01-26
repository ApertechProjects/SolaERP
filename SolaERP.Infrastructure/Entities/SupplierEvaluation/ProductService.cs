using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class ProductService : BaseEntity
    {
        [DbColumn("ProductServiceId")]
        public int Id { get; set; }
        [DbColumn("ProductServiceName")]
        public string Name { get; set; }
        public int Other { get; set; }
        public int BusinessCategoryId { get; set; }
    }
}

using SolaERP.Application.Entities.Status;

namespace SolaERP.Application.Models
{
    public class VendorFilter
    {
        public List<int> PrequalificationCategoryId { get; set; }
        public List<int> BusinessCategoryId { get; set; }
        public List<int> ProductServiceId { get; set; }
        public List<int> VendorTypeId { get; set; }
        public string Text { get; set; }
    }


}

namespace SolaERP.Application.Entities.Vendors
{
	public class VendorBusinessCategory : BaseEntity
	{
		public int VendorBusinessCategoryId { get; set; }
		public int VendorId { get; set; }
		public int BusinessCategoryId { get; set; }
		public bool Active { get; set; }

	}
}

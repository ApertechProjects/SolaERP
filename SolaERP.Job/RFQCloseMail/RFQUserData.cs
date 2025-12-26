namespace SolaERP.Job.RFQClose
{
	public class RFQUserData
	{
		public int RFQMainId { get; set; }
		public string VendorName { get; set; }
		public string Email { get; set; }
		public string RFQNo { get; set; }
		public string BusinessUnitName { get; set; }
		public DateTime RFQDeadline { get; set; }
        public int RFQVendorResponseId { get; set; }
        public string Language { get; set; }
		public string VendorCode { get; set; }
		public int UserId { get; set; } = 0;
	}
}

namespace RetailDistributionClient.UI.Model
{
	public class District
	{
		public int DistrictId { get; set; }
		public string DistrictName { get; set; }
		public virtual Vendor PrimaryVendor { get; set; }
	}
}

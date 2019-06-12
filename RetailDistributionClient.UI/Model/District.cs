namespace RetailDistribution.Client.UI.Model
{
    public class District
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public Vendor PrimaryVendor { get; set; }
    }
}

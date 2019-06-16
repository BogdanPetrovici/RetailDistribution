using RetailDistribution.Data;
using RetailDistribution.Data.Model;
using System.Data.Entity;

namespace RetailDistribution.Web.Test.Mocks
{
	public class TestRetailDistributionContext : IRetailDistributionContext
	{
		public TestRetailDistributionContext()
		{
			this.Districts = new TestDistrictDbSet();
			this.Vendors = new TestVendorDbSet();
			this.Shops = new TestShopDbSet();
			this.DistrictVendors = new TestDistrictVendorDbSet();
		}

		public DbSet<District> Districts { get; set; }
		public DbSet<Shop> Shops { get; set; }
		public DbSet<Vendor> Vendors { get; set; }
		public DbSet<DistrictVendor> DistrictVendors { get; set; }

		public int SaveChanges()
		{
			return 0;
		}

		public void MarkAsModified(District item) { }
		public void Dispose() { }
	}
}

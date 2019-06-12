using System.Data.Entity;

namespace RetailDistribution.Data.Initializers
{
	public class RetailDistributionDbInitializer : CreateDatabaseIfNotExists<RetailDistributionContext>
	{
		public override void InitializeDatabase(RetailDistributionContext context)
		{
			base.InitializeDatabase(context);
		}

		protected override void Seed(RetailDistributionContext context)
		{
			base.Seed(context);
			var vendor1 = context.Vendors.Add(new Model.Vendor { VendorName = "Vendor1" });
			var vendor2 = context.Vendors.Add(new Model.Vendor { VendorName = "Vendor2" });
			var vendor3 = context.Vendors.Add(new Model.Vendor { VendorName = "Vendor3" });

			var district1 = context.Districts.Add(new Model.District { DistrictName = "District1", PrimaryVendor = vendor2 });
			var district2 = context.Districts.Add(new Model.District { DistrictName = "District2", PrimaryVendor = vendor2 });
			var district3 = context.Districts.Add(new Model.District { DistrictName = "District3", PrimaryVendor = vendor1 });

			context.Shops.Add(new Model.Shop { ShopName = "Shop1", District = district1 });
			context.Shops.Add(new Model.Shop { ShopName = "Shop2", District = district2 });
			context.Shops.Add(new Model.Shop { ShopName = "Shop3", District = district1 });

			context.DistrictVendors.Add(new Model.DistrictVendor { Vendor = vendor3, District = district3 });
		}
	}
}

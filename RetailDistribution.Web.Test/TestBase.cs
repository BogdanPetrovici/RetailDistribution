using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using RetailDistribution.Web.Test.Mocks;

namespace RetailDistribution.Web.Test
{
	public class TestBase
	{
		protected IRetailDistributionUnitOfWork GetUnitOfWork()
		{
			TestRetailDistributionContext contextMock = new TestRetailDistributionContext();
			var vendor1 = contextMock.Vendors.Add(new Vendor { VendorId = 1, VendorName = "Vendor1" });
			var vendor2 = contextMock.Vendors.Add(new Vendor { VendorId = 2, VendorName = "Vendor2" });
			var vendor3 = contextMock.Vendors.Add(new Vendor { VendorId = 3, VendorName = "Vendor3" });

			var district1 = contextMock.Districts.Add(new District { DistrictId = 1, DistrictName = "District1", PrimaryVendor = vendor2 });
			var district2 = contextMock.Districts.Add(new District { DistrictId = 2, DistrictName = "District2", PrimaryVendor = vendor2 });
			var district3 = contextMock.Districts.Add(new District { DistrictId = 3, DistrictName = "District3", PrimaryVendor = vendor1 });

			contextMock.Shops.Add(new Shop { ShopId = 1, ShopName = "Shop1", District = district1 });
			contextMock.Shops.Add(new Shop { ShopId = 2, ShopName = "Shop2", District = district2 });
			contextMock.Shops.Add(new Shop { ShopId = 3, ShopName = "Shop3", District = district1 });

			contextMock.DistrictVendors.Add(new DistrictVendor { VendorId = 3, Vendor = vendor3, District = district3, DistrictId = 3 });
			contextMock.DistrictVendors.Add(new DistrictVendor { VendorId = 2, Vendor = vendor2, District = district1, DistrictId = 1 });
			contextMock.DistrictVendors.Add(new DistrictVendor { VendorId = 2, Vendor = vendor2, District = district2, DistrictId = 2 });
			contextMock.DistrictVendors.Add(new DistrictVendor { VendorId = 1, Vendor = vendor1, District = district3, DistrictId = 3 });

			var districtRepository = new DistrictRepository(contextMock);
			var vendorRepository = new VendorRepository(contextMock);
			var shopRepository = new ShopRepository(contextMock);

			return new RetailDistributionUnitOfWork(contextMock, districtRepository, vendorRepository, shopRepository);
		}
	}
}

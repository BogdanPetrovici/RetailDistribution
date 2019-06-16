using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using RetailDistribution.Web.Controllers;
using RetailDistribution.Web.Test.Mocks;

namespace RetailDistribution.Web.Test
{
	[TestClass]
	public class TestDistrictController
	{
		[TestMethod]
		public void Get_ShouldReturnAllDistricts()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.Get() as TestDistrictDbSet;
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Local.Count);
			//Make sure the repository includes the primary vendor in the result
			foreach (var district in result)
			{
				Assert.IsNotNull(district.PrimaryVendor);
			}
		}

		private IRetailDistributionUnitOfWork GetUnitOfWork()
		{
			TestRetailDistributionContext contextMock = new TestRetailDistributionContext();
			var vendor1 = contextMock.Vendors.Add(new Vendor { VendorName = "Vendor1" });
			var vendor2 = contextMock.Vendors.Add(new Vendor { VendorName = "Vendor2" });
			var vendor3 = contextMock.Vendors.Add(new Vendor { VendorName = "Vendor3" });

			var district1 = contextMock.Districts.Add(new District { DistrictName = "District1", PrimaryVendor = vendor2 });
			var district2 = contextMock.Districts.Add(new District { DistrictName = "District2", PrimaryVendor = vendor2 });
			var district3 = contextMock.Districts.Add(new District { DistrictName = "District3", PrimaryVendor = vendor1 });

			contextMock.Shops.Add(new Shop { ShopName = "Shop1", District = district1 });
			contextMock.Shops.Add(new Shop { ShopName = "Shop2", District = district2 });
			contextMock.Shops.Add(new Shop { ShopName = "Shop3", District = district1 });

			contextMock.DistrictVendors.Add(new DistrictVendor { Vendor = vendor3, District = district3 });
			contextMock.DistrictVendors.Add(new DistrictVendor { Vendor = vendor2, District = district1 });
			contextMock.DistrictVendors.Add(new DistrictVendor { Vendor = vendor2, District = district2 });
			contextMock.DistrictVendors.Add(new DistrictVendor { Vendor = vendor1, District = district3 });

			var districtRepository = new DistrictRepository(contextMock);
			var vendorRepository = new VendorRepository(contextMock);
			var shopRepository = new ShopRepository(contextMock);

			return new RetailDistributionUnitOfWork(contextMock, districtRepository, vendorRepository, shopRepository);
		}
	}
}

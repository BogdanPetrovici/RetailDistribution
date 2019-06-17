using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailDistribution.Client.UI.Model;
using RetailDistribution.Client.UI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.Test
{
	[TestClass]
	public class TestMainViewModel : TestBase
	{
		[TestMethod]
		public async Task GetDistricts_ReturnsExpectedListOfDistricts()
		{
			var httpClient = GetMockedHttpClient(ExpectedGetDistrictsResultString);
			var viewModel = new MainViewModel(null, httpClient);
			Assert.IsNull(viewModel.Districts);
			var result = await viewModel.GetDistrictsAsync(ServicePaths.DistrictsEndpoint);

			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.ToList().Count);
		}

		[TestMethod]
		public async Task GetVendors_ReturnsExpectedListOfVendors()
		{
			var httpClient = GetMockedHttpClient(ExpectedGetVendorsResultString);
			var viewModel = new MainViewModel(null, httpClient);
			Assert.IsNull(viewModel.Vendors);
			viewModel.SelectedDistrict = new District { DistrictId = 3 };

			var result = await viewModel.GetVendorsAsync(ServicePaths.DistrictsEndpoint);

			Assert.IsNotNull(viewModel.Vendors);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.ToList().Count);
		}

		[TestMethod]
		public async Task GetVendors_WithoutSelectingDistrict_ReturnsNothing()
		{
			var httpClient = GetMockedHttpClient(ExpectedGetVendorsResultString);
			var viewModel = new MainViewModel(null, httpClient);
			Assert.IsNull(viewModel.Vendors);
			viewModel.SelectedDistrict = null;

			var result = await viewModel.GetVendorsAsync(ServicePaths.DistrictsEndpoint);

			Assert.IsNull(viewModel.Vendors);
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task GetShops_WithoutSelectingDistrict_ReturnsNothing()
		{
			var httpClient = GetMockedHttpClient(ExpectedGetShopsResultString);
			var viewModel = new MainViewModel(null, httpClient);
			Assert.IsNull(viewModel.Shops);
			viewModel.SelectedDistrict = null;

			var result = await viewModel.GetVendorsAsync(ServicePaths.ShopsEndpoint);

			Assert.IsNull(viewModel.Shops);
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task GetVendors_ReturnsExpectedListOfShops()
		{
			var httpClient = GetMockedHttpClient(ExpectedGetShopsResultString);
			var viewModel = new MainViewModel(null, httpClient);
			Assert.IsNull(viewModel.Shops);
			viewModel.SelectedDistrict = new District { DistrictId = 1 };

			var result = await viewModel.GetShopsAsync(ServicePaths.ShopsEndpoint);

			Assert.IsNotNull(viewModel.Shops);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.ToList().Count);
		}
	}
}
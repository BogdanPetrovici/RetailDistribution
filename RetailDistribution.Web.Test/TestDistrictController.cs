using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailDistribution.Data.Model;
using RetailDistribution.Web.Controllers;
using RetailDistribution.Web.Test.Mocks;
using System.Linq;
using System.Web.Http.Results;

namespace RetailDistribution.Web.Test
{
	[TestClass]
	public class TestDistrictController : TestBase
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

		[TestMethod]
		public void GetVendors_ShouldReturnCorrespondingVendors()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.GetVendors(3);
			Assert.IsNotNull(result);
			var resultList = result.ToList();
			Assert.AreEqual(2, resultList.Count);
			//Check that one of the vendors is marked as primary
			Assert.IsTrue(resultList[1].IsPrimary);
		}

		[TestMethod]
		public void GetVendors_ShouldReturnUniquePrimaryVendor()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.GetVendors(3);
			Assert.IsNotNull(result);
			int primaryVendors = 0;
			foreach (var vendor in result)
			{
				if (vendor.IsPrimary) { primaryVendors++; }
			}

			Assert.AreEqual(1, primaryVendors);
		}

		[TestMethod]
		public void GetRemainingVendors_ReturnsUnusedVendors()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.GetRemainingVendors(3);
			Assert.IsNotNull(result);
			var resultList = result.ToList();
			Assert.AreEqual(1, resultList.Count);
			Assert.AreEqual(2, resultList.First().VendorId);
		}

		[TestMethod]
		public void Put_GetsNullParameter_ReturnsNull()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.Put(null);
			Assert.IsInstanceOfType(result, typeof(BadRequestResult));
		}

		[TestMethod]
		public void Put_ReturnsDistrictWithUpdatedPrimaryVendor()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.Put(new District { DistrictId = 3, PrimaryVendor = new Vendor { VendorId = 1 } })
							as OkNegotiatedContentResult<District>;
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.PrimaryVendor.VendorId);
		}

		[TestMethod]
		public void RemoveVendor_ReturnsTrueIfSuccessful()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.RemoveVendor(3, 3) as OkNegotiatedContentResult<bool>;
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Content);
		}

		[TestMethod]
		public void RemoveVendor_GetsPrimaryVendor_ReturnsBadRequest()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new DistrictController(unitOfWork);

			var result = controller.RemoveVendor(3, 1) as BadRequestErrorMessageResult;
			Assert.IsNotNull(result);
		}
	}
}

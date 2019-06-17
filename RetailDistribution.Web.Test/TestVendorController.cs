using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailDistribution.Data.Model;
using RetailDistribution.Web.Controllers;
using System.Linq;
using System.Web.Http.Results;

namespace RetailDistribution.Web.Test
{
	[TestClass]
	public class TestVendorController : TestBase
	{
		[TestMethod]
		public void Post_ReceivesNonPrimaryVendor_ReturnsOkStatusCode()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(1, new Vendor { VendorId = 3 }) as OkNegotiatedContentResult<bool>;
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Content);
		}

		[TestMethod]
		public void Post_ReceivesNonPrimaryVendor_RegistersVendor()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(1, new Vendor { VendorId = 3 }) as OkNegotiatedContentResult<bool>;
			var dbVendors = unitOfWork.VendorRepository.GetVendors(1).ToList();
			Assert.AreEqual(2, dbVendors.Count);
			foreach (var vendor in dbVendors)
			{
				if (vendor.VendorId == 3)
				{
					//Added vendor is not marked as primary
					Assert.IsFalse(vendor.IsPrimary);
				}
			}
		}

		[TestMethod]
		public void Post_ReceivesPrimaryVendor_ReturnsOkStatusCode()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(1, new Vendor { VendorId = 3, IsPrimary = true }) as OkNegotiatedContentResult<bool>;
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Content);
		}

		[TestMethod]
		public void Post_ReceivesPrimaryVendor_RegistersVendor()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(1, new Vendor { VendorId = 3, IsPrimary = true }) as OkNegotiatedContentResult<bool>;
			var dbVendors = unitOfWork.VendorRepository.GetVendors(1).ToList();
			Assert.AreEqual(2, dbVendors.Count);
			foreach (var vendor in dbVendors)
			{
				if (vendor.VendorId == 3)
				{
					//Added vendor is not marked as primary
					Assert.IsTrue(vendor.IsPrimary);
				}
			}
		}

		[TestMethod]
		public void Post_ReceivesInvalidDistrict_ReturnsAssociatedVendors()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(4, new Vendor { VendorId = 3 }) as BadRequestResult;
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Post_ReceivesInvalidVendor_ReturnsAssociatedVendors()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new VendorController(unitOfWork);

			var result = controller.Post(3, new Vendor { VendorId = 4 }) as BadRequestResult;
			Assert.IsNotNull(result);
		}
	}
}

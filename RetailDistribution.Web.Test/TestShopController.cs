using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailDistribution.Web.Controllers;
using System.Linq;

namespace RetailDistribution.Web.Test
{
	[TestClass]
	public class TestShopController : TestBase
	{
		[TestMethod]
		public void Get_ReturnsAssociatedShops()
		{
			var unitOfWork = GetUnitOfWork();
			var controller = new ShopController(unitOfWork);

			var result = controller.Get(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.ToList().Count);
		}
	}
}

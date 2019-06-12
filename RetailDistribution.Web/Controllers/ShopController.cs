using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;

namespace RetailDistribution.Web.Controllers
{
	public class ShopController : ApiController
	{
		private RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork();

		public IEnumerable<Shop> Get(int id)
		{
			var shops = unitOfWork.ShopRepository.GetShops(id);
			return shops;
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

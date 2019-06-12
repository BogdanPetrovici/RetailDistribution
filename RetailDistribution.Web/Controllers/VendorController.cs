using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;

namespace RetailDistribution.Web.Controllers
{
	public class VendorController : ApiController
	{
		private RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork();

		/// <summary>
		/// Gets the vendors associated with a specified district id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Vendor> Get(int id)
		{
			var vendors = unitOfWork.VendorRepository.GetVendors(id);
			return vendors;
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

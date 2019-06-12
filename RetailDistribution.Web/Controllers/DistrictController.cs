using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;

namespace RetailDistribution.Web.Controllers
{
	public class DistrictController : ApiController
	{
		private RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork();

		public IEnumerable<District> Get()
		{
			var districts = unitOfWork.DistrictRepository.GetDistricts();
			return districts;
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

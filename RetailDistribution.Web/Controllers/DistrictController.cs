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

		/// <summary>
		/// Receives a deserialized version of the UI model, gets the database entity equivalents and updates 
		/// the district's corresponding primary vendor
		/// </summary>
		/// <param name="district"></param>
		/// <returns></returns>
		public District Put(District district)
		{
			if (district != null && district.PrimaryVendor != null)
			{
				var districtEntity = unitOfWork.DistrictRepository.GetDistrict(district.DistrictId);
				var vendorEntity = unitOfWork.VendorRepository.GetVendor(district.PrimaryVendor.VendorId);
				if (districtEntity != null && vendorEntity != null)
				{
					districtEntity.PrimaryVendor = vendorEntity;
				}

				unitOfWork.Save();
				return districtEntity;
			}

			return null;
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

using RetailDistribution.Data.Model;
using System;
using System.Data.Entity;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public class DistrictRepository : IDistrictRepository
	{
		private IRetailDistributionContext context;

		public DistrictRepository(IRetailDistributionContext context)
		{
			this.context = context;
		}

		public District GetDistrict(int id)
		{
			return context.Districts.FirstOrDefault(d => d.DistrictId == id);
		}

		/// <summary>
		/// Gets a list of all tracked districts
		/// </summary>
		/// <returns>List of type <see cref="District"/></returns>
		public IQueryable<District> GetDistricts()
		{
			return context.Districts.Include(d => d.PrimaryVendor);
		}

		/// <summary>
		/// Removes the vendor from its associated district
		/// </summary>
		/// <param name="districtId"></param>
		/// <param name="vendorId"></param>
		/// <returns></returns>
		public void RemoveVendor(int districtId, int vendorId)
		{
			DistrictVendor dvEntity = context.DistrictVendors.Include(dv => dv.District).Include(dv => dv.District.PrimaryVendor).FirstOrDefault(dv => dv.DistrictId == districtId && dv.VendorId == vendorId);
			//Only remove relationships where the vendor is not primary
			//if the vendor is primary, it must be set as secondary first
			if (dvEntity != null && dvEntity.District.PrimaryVendor.VendorId != vendorId)
			{
				context.DistrictVendors.Remove(dvEntity);
			}
			else
			{
				throw new InvalidOperationException("Cannot remove primary vendor.");
			}
		}
	}
}

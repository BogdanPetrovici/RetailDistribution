using RetailDistribution.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public class VendorRepository : IVendorRepository
	{
		private RetailDistributionContext context;

		public VendorRepository(RetailDistributionContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Gets a list of all vendors associated with a particular district
		/// </summary>
		/// <param name="districtId">The vendor's id</param>
		/// <returns></returns>
		public IEnumerable<Vendor> GetVendors(int districtId)
		{
			var district = context.Districts.Where(d => d.DistrictId == districtId)
											.Include(d => d.PrimaryVendor)
											.SingleOrDefault();
			if (district != null)
			{
				var vendors = context.DistrictVendors.Where(dv => dv.DistrictId == districtId)
										.Include(dv => dv.Vendor)
										.Select(dv => dv.Vendor)
										.ToList();
				vendors.Add(district.PrimaryVendor);
				return vendors;
			}

			return null;
		}
	}
}

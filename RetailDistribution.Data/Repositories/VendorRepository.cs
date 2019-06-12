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
		/// Gets a particular vendor, specified via its id
		/// </summary>
		/// <param name="vendorId">The vendor's id</param>
		/// <returns>The <see cref="Vendor"/> entity, if found and null otherwise</returns>
		public Vendor GetVendor(int vendorId)
		{
			return context.Vendors.FirstOrDefault(v => v.VendorId == vendorId);
		}

		/// <summary>
		/// Gets a list of all vendors associated with a particular district
		/// </summary>
		/// <param name="districtId">The district's id</param>
		/// <returns>The list of <see cref="Vendor"/> entities associated to the given district id</returns>
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
				// Assuming that the object reference in PrimaryVendor 
				// is the same as the corresponding vendor's object 
				// reference in the vendors list (because of EF magic)
				district.PrimaryVendor.IsPrimary = true;
				return vendors;
			}

			return null;
		}
	}
}

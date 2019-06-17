using RetailDistribution.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public interface IVendorRepository
	{
		IQueryable<Vendor> GetVendors(int districtId);
		Vendor GetVendor(int vendorId);
		bool AddVendor(int districtId, Vendor vendor);
		IEnumerable<Vendor> GetRemainingVendors(int districtId);
	}
}

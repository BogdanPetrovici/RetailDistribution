using RetailDistribution.Data.Model;
using System.Collections.Generic;

namespace RetailDistribution.Data.Repositories
{
	public interface IVendorRepository
	{
		IEnumerable<Vendor> GetVendors(int districtId);
		Vendor GetVendor(int vendorId);
	}
}

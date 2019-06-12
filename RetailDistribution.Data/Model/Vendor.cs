using System.Collections.Generic;

namespace RetailDistribution.Data.Model
{
	public class Vendor
	{
		public virtual int VendorId { get; set; }
		public virtual string VendorName { get; set; }
		public virtual IEnumerable<District> Districts { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailDistribution.Data.Model
{
	public class DistrictVendor
	{
		[Key, Column(Order = 0)]
		public virtual int DistrictId { get; set; }
		[Key, Column(Order = 1)]
		public virtual int VendorId { get; set; }
		[Required]
		public virtual District District { get; set; }
		[Required]
		public virtual Vendor Vendor { get; set; }
	}
}

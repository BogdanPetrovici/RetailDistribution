using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailDistribution.Data.Model
{
	public class District
	{
		public virtual int DistrictId { get; set; }
		public virtual string DistrictName { get; set; }
		public virtual IEnumerable<Vendor> Vendors { get; set; }
		[Required]
		public virtual Vendor PrimaryVendor { get; set; }
	}
}

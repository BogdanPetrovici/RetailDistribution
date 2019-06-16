using RetailDistribution.Data.Model;
using System.Linq;

namespace RetailDistribution.Web.Test.Mocks
{
	public class TestDistrictVendorDbSet : TestDbSet<DistrictVendor>
	{
		public override DistrictVendor Find(params object[] keyValues)
		{
			return this.SingleOrDefault(dv => dv.DistrictId == (int)keyValues.Single());
		}
	}
}

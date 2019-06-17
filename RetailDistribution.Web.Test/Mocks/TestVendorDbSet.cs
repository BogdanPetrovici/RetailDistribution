using RetailDistribution.Data.Model;
using System.Linq;

namespace RetailDistribution.Web.Test.Mocks
{
	public class TestVendorDbSet : TestDbSet<Vendor>
	{
		public override Vendor Find(params object[] keyValues)
		{
			return this.SingleOrDefault(v => v.VendorId == (int)keyValues.Single());
		}
	}
}

using RetailDistribution.Data.Model;
using System.Linq;

namespace RetailDistribution.Web.Test.Mocks
{
	public class TestShopDbSet : TestDbSet<Shop>
	{
		public override Shop Find(params object[] keyValues)
		{
			return this.SingleOrDefault(s => s.ShopId == (int)keyValues.Single());
		}
	}
}
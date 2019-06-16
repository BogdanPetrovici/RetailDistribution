using RetailDistribution.Data.Model;
using System.Linq;

namespace RetailDistribution.Web.Test.Mocks
{
	public class TestDistrictDbSet : TestDbSet<District>
	{
		public override District Find(params object[] keyValues)
		{
			return this.SingleOrDefault(district => district.DistrictId == (int)keyValues.Single());
		}
	}
}

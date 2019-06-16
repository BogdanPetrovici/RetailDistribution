using RetailDistribution.Data.Model;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public class ShopRepository : IShopRepository
	{
		private IRetailDistributionContext context;

		public ShopRepository(IRetailDistributionContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Gets a list of all shops belonging to a specified district
		/// </summary>
		/// <param name="districtId">The district's id</param>
		/// <returns></returns>
		public IQueryable<Shop> GetShops(int districtId)
		{
			return context.Shops.Where(s => s.District.DistrictId == districtId);
		}
	}
}

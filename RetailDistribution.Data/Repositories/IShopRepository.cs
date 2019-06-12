using RetailDistribution.Data.Model;
using System.Collections.Generic;

namespace RetailDistribution.Data.Repositories
{
	public interface IShopRepository
	{
		/// <summary>
		/// Gets a list of all shops belonging to a specified district
		/// </summary>
		/// <param name="districtId">The district's id</param>
		/// <returns></returns>
		IEnumerable<Shop> GetShops(int districtId);
	}
}

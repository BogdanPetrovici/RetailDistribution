using RetailDistribution.Data.Model;
using System.Collections.Generic;

namespace RetailDistribution.Data.Repositories
{
	public interface IDistrictRepository
	{
		/// <summary>
		/// Gets a list of all tracked districts
		/// </summary>
		/// <returns>List of type <see cref="District"/></returns>
		IEnumerable<District> GetDistricts();
	}
}

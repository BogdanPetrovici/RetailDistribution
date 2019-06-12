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

		/// <summary>
		/// Gets a particular district, specified by its id
		/// </summary>
		/// <param name="id">The district id</param>
		/// <returns>An object of type <see cref="District"/> if the id is valid, or null, otherwise</returns>
		District GetDistrict(int id);
	}
}

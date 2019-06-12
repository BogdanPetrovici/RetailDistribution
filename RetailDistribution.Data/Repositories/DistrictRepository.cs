using RetailDistribution.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public class DistrictRepository : IDistrictRepository
	{
		private RetailDistributionContext context;

		public DistrictRepository(RetailDistributionContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Gets a list of all tracked districts
		/// </summary>
		/// <returns>List of type <see cref="District"/></returns>
		public IEnumerable<District> GetDistricts()
		{
			return context.Districts.Include(d => d.PrimaryVendor).ToList();
		}
	}
}

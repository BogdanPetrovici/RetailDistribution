using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Collections.Generic;

namespace RetailDistribution.Console
{
	public class Program
	{
		static void Main(string[] args)
		{
			using (RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork())
			{
				IEnumerable<District> districts = unitOfWork.DistrictRepository.GetDistricts();
			}
		}
	}
}

using RetailDistribution.Data.Model;
using System;
using System.Data.Entity;

namespace RetailDistribution.Data
{
	public interface IRetailDistributionContext : IDisposable
	{
		DbSet<Shop> Shops { get; set; }
		DbSet<Vendor> Vendors { get; set; }
		DbSet<District> Districts { get; set; }
		DbSet<DistrictVendor> DistrictVendors { get; set; }
		int SaveChanges();
	}
}

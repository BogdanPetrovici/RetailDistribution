using RetailDistribution.Data.Initializers;
using RetailDistribution.Data.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RetailDistribution.Data
{
	public class RetailDistributionContext : DbContext
	{
		public RetailDistributionContext() : base("name=RetailDistributionConnectionString")
		{
			Database.SetInitializer<RetailDistributionContext>(new RetailDistributionDbInitializer());
			Configuration.ProxyCreationEnabled = false;
			Configuration.LazyLoadingEnabled = false;
		}

		public DbSet<Shop> Shops { get; set; }
		public DbSet<Vendor> Vendors { get; set; }
		public DbSet<District> Districts { get; set; }
		public DbSet<DistrictVendor> DistrictVendors { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//EF doesn't want to allow me to have a foreign key for the primary vendor on the Districts table, 
			// and the many-to-many relationship as well, without adding this - don't intend to do cascade deletes, so should be fine
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}
	}
}

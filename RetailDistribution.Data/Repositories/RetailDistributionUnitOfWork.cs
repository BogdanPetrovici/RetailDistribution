using System;

namespace RetailDistribution.Data.Repositories
{
	/// <summary>
	/// Not really necessary, but for conceptual consistency there were created multiple repositories, 
	/// therefore a unit of work for saving and disposing the unique context would be necessary
	/// </summary>
	public class RetailDistributionUnitOfWork : IRetailDistributionUnitOfWork
	{
		private IDistrictRepository districtRepository;
		private IShopRepository shopRepository;
		private IVendorRepository vendorRepository;
		private readonly RetailDistributionContext context;

		public RetailDistributionUnitOfWork(RetailDistributionContext context, IDistrictRepository districtRepository, IVendorRepository vendorRepository, IShopRepository shopRepository)
		{
			this.districtRepository = districtRepository;
			this.vendorRepository = vendorRepository;
			this.shopRepository = shopRepository;
			this.context = context;
		}

		public IDistrictRepository DistrictRepository
		{
			get { return districtRepository; }
		}

		public IVendorRepository VendorRepository
		{
			get { return vendorRepository; }
		}

		public IShopRepository ShopRepository
		{
			get { return shopRepository; }
		}

		public void Save()
		{
			context.SaveChanges();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}

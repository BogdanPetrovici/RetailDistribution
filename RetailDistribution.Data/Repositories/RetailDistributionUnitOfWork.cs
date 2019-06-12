using System;

namespace RetailDistribution.Data.Repositories
{
	/// <summary>
	/// Not really necessary, but for conceptual consistency there were created multiple repositories, 
	/// therefore a unit of work for saving and disposing the unique context would be necessary
	/// </summary>
	public class RetailDistributionUnitOfWork : IDisposable
	{
		private RetailDistributionContext context = new RetailDistributionContext();
		private IDistrictRepository districtRepository;
		private IShopRepository shopRepository;
		private IVendorRepository vendorRepository;

		public IDistrictRepository DistrictRepository
		{
			get
			{
				if (districtRepository == null)
				{
					districtRepository = new DistrictRepository(context);
				}

				return districtRepository;
			}
		}

		public IVendorRepository VendorRepository
		{
			get
			{
				if (vendorRepository == null)
				{
					vendorRepository = new VendorRepository(context);
				}

				return vendorRepository;
			}
		}

		public IShopRepository ShopRepository
		{
			get
			{
				if (shopRepository == null)
				{
					shopRepository = new ShopRepository(context);
				}

				return shopRepository;
			}
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

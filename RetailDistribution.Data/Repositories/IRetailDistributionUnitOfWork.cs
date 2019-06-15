using System;

namespace RetailDistribution.Data.Repositories
{
	public interface IRetailDistributionUnitOfWork : IDisposable
	{
		IDistrictRepository DistrictRepository { get; }
		IVendorRepository VendorRepository { get; }
		IShopRepository ShopRepository { get; }
		void Save();
	}
}

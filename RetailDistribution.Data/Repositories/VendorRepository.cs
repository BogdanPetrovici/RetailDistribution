using RetailDistribution.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RetailDistribution.Data.Repositories
{
	public class VendorRepository : IVendorRepository
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IRetailDistributionContext context;

		public VendorRepository(IRetailDistributionContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Adds a vendor to an existing district
		/// </summary>
		/// <param name="districtId">The district id to which the vendor will be associated</param>
		/// <param name="vendor">Transfer object sent from the client (in this case, the client's model becomes an unlinked EF model)</param>
		/// <returns>true if everything ok, false otherwise</returns>
		public bool AddVendor(int districtId, Vendor vendor)
		{
			try
			{
				if (vendor != null)
				{
					var vendorEntity = context.Vendors.FirstOrDefault(v => v.VendorId == vendor.VendorId);
					// If a district was selected when adding the vendor, create the link between the two
					if (vendorEntity != null)
					{
						var districtEntity = context.Districts.FirstOrDefault(d => d.DistrictId == districtId);
						if (districtEntity != null)
						{
							context.DistrictVendors.Add(new DistrictVendor
							{
								District = districtEntity,
								Vendor = vendorEntity
							});

							// If the vendor was marked as primary, mark this in the corresponding district
							if (vendor.IsPrimary)
							{
								districtEntity.PrimaryVendor = vendorEntity;
							}
						}
						else { return false; }
					}
					else { return false; }
				}

				return true;
			}
			catch (Exception ex)
			{
				log.Error(ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// Gets a particular vendor, specified via its id
		/// </summary>
		/// <param name="vendorId">The vendor's id</param>
		/// <returns>The <see cref="Vendor"/> entity, if found and null otherwise</returns>
		public Vendor GetVendor(int vendorId)
		{
			return context.Vendors.FirstOrDefault(v => v.VendorId == vendorId);
		}

		/// <summary>
		/// Gets a list of all vendors associated with a particular district
		/// </summary>
		/// <param name="districtId">The district's id</param>
		/// <returns>The list of <see cref="Vendor"/> entities associated to the given district id</returns>
		public IQueryable<Vendor> GetVendors(int districtId)
		{
			var district = context.Districts.Where(d => d.DistrictId == districtId)
											.Include(d => d.PrimaryVendor)
											.SingleOrDefault();
			if (district != null)
			{
				var vendors = context.DistrictVendors
										.Include(dv => dv.District)
										.Where(dv => dv.District.DistrictId == districtId)
										.Include(dv => dv.Vendor)
										.Select(dv => dv.Vendor);
				// Assuming that the object reference in PrimaryVendor 
				// is the same as the corresponding vendor's object 
				// reference in the vendors list (because of EF magic)
				district.PrimaryVendor.IsPrimary = true;
				return vendors;
			}

			return null;
		}

		/// <summary>
		/// Gets a list of vendors not related to the specified district id
		/// </summary>
		/// <param name="districtId">The district's id</param>
		/// <returns>A list of <see cref="Vendor"/> entities</returns>
		public IEnumerable<Vendor> GetRemainingVendors(int districtId)
		{
			// first we select all vendors related to this districtId
			var relatedVendorIds = context.DistrictVendors.Where(dv => dv.DistrictId == districtId)
																			.Select(dv => dv.VendorId).ToList();
			// then we select only the vendors not present in the list
			var vendors = context.Vendors.Where(v => !relatedVendorIds.Contains(v.VendorId)).ToList();
			return vendors;
		}
	}
}

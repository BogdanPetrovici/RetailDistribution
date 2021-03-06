﻿using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace RetailDistribution.Web.Controllers
{
	public class DistrictController : ApiController
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IRetailDistributionUnitOfWork unitOfWork;

		public DistrictController(IRetailDistributionUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IQueryable<District> Get()
		{
			var districts = unitOfWork.DistrictRepository.GetDistricts();
			return districts;
		}

		/// <summary>
		/// Gets the vendors associated with a specified district id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IQueryable<Vendor> GetVendors(int id)
		{
			var vendors = unitOfWork.VendorRepository.GetVendors(id);
			return vendors;
		}

		/// <summary>
		/// Gets a list of all vendors not currently associated with the specified id
		/// </summary>
		/// <param name="id">The district id</param>
		/// <returns>A list of <see cref="Vendor"/> entities</returns>
		public IEnumerable<Vendor> GetRemainingVendors(int id)
		{
			var vendors = unitOfWork.VendorRepository.GetRemainingVendors(id);
			return vendors;
		}

		/// <summary>
		/// Receives a deserialized version of the UI model, gets the database entity equivalents and updates 
		/// the district's corresponding primary vendor
		/// </summary>
		/// <param name="district"></param>
		/// <returns></returns>
		[ResponseType(typeof(District))]
		public IHttpActionResult Put([FromBody]District district)
		{
			if (district != null && district.PrimaryVendor != null)
			{
				var districtEntity = unitOfWork.DistrictRepository.GetDistrict(district.DistrictId);
				var vendorEntity = unitOfWork.VendorRepository.GetVendor(district.PrimaryVendor.VendorId);
				if (districtEntity != null && vendorEntity != null)
				{
					districtEntity.PrimaryVendor = vendorEntity;
				}

				unitOfWork.Save();
				return Ok(districtEntity);
			}

			return BadRequest();
		}

		/// <summary>
		/// Removes the specified vendor from its linked district
		/// </summary>
		/// <param name="districtId">The district for which we want to remove the vendor</param>
		/// <param name="vendorId">The id of the vendor that must be removed</param>
		/// <returns>True if successful, false, otherwise</returns>
		[HttpDelete]
		[ResponseType(typeof(bool))]
		[Route("api/district/removeVendor/{districtId}/{vendorId}")]
		public IHttpActionResult RemoveVendor(int districtId, int vendorId)
		{
			try
			{
				unitOfWork.DistrictRepository.RemoveVendor(districtId, vendorId);
				unitOfWork.Save();
				return Ok(true);
			}
			catch (InvalidOperationException exCannotRemove)
			{
				log.Error(exCannotRemove.ToString());
				return BadRequest(exCannotRemove.Message);
			}
			catch (Exception ex)
			{
				log.Error(ex.ToString());
				return InternalServerError(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

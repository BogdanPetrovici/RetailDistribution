using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailDistribution.Web.Controllers
{
    public class DistrictController : ApiController
    {
        private RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork();

        public IEnumerable<District> Get()
        {
            var districts = unitOfWork.DistrictRepository.GetDistricts();
            return districts;
        }

        /// <summary>
        /// Gets the vendors associated with a specified district id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Vendor> GetVendors(int id)
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
        public District Put([FromBody]District district)
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
                return districtEntity;
            }

            return null;
        }

        /// <summary>
        /// Removes the specified vendor from its linked district
        /// </summary>
        /// <param name="districtId">The district for which we want to remove the vendor</param>
        /// <param name="vendorId">The id of the vendor that must be removed</param>
        /// <returns>True if successful, false, otherwise</returns>
        [HttpDelete]
        [Route("api/district/removeVendor/{districtId}/{vendorId}")]
        public HttpResponseMessage RemoveVendor(int districtId, int vendorId)
        {
            try
            {
                unitOfWork.DistrictRepository.RemoveVendor(districtId, vendorId);
                unitOfWork.Save();
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (InvalidOperationException exCannotRemove)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, exCannotRemove.Message);
            }
            catch (Exception ex)
            {
                //log exception
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}

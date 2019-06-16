using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Web.Http;
using System.Web.Http.Description;

namespace RetailDistribution.Web.Controllers
{
	public class VendorController : ApiController
	{
		private IRetailDistributionUnitOfWork unitOfWork;

		public VendorController(IRetailDistributionUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Adds a new vendor to the database
		/// </summary>
		/// <param name="id">The district id to which the vendor will be associated</param>
		/// <param name="vendor">The deserialization of the transfer object sent by the client. Should contain the district id and vendor name</param>
		/// <returns>True, if successful, false otherwise</returns>
		[ResponseType(typeof(bool))]
		public IHttpActionResult Post(int id, [FromBody]Vendor vendor)
		{
			if (unitOfWork.VendorRepository.AddVendor(id, vendor))
			{
				unitOfWork.Save();
				return Ok(true);
			}

			return BadRequest();
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

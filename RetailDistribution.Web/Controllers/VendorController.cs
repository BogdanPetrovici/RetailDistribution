using RetailDistribution.Data.Model;
using RetailDistribution.Data.Repositories;
using System.Web.Http;

namespace RetailDistribution.Web.Controllers
{
	public class VendorController : ApiController
	{
		private RetailDistributionUnitOfWork unitOfWork = new RetailDistributionUnitOfWork();

		/// <summary>
		/// Adds a new vendor to the database
		/// </summary>
		/// <param name="id">The district id to which the vendor will be associated</param>
		/// <param name="vendor">The deserialization of the transfer object sent by the client. Should contain the district id and vendor name</param>
		/// <returns>True, if successful, false otherwise</returns>
		public bool Post(int id, [FromBody]Vendor vendor)
		{
			if (unitOfWork.VendorRepository.AddVendor(id, vendor))
			{
				unitOfWork.Save();
				return true;
			}

			return false;
		}

		protected override void Dispose(bool disposing)
		{
			unitOfWork.Dispose();
			base.Dispose(disposing);
		}
	}
}

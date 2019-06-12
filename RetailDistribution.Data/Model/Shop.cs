namespace RetailDistribution.Data.Model
{
	public class Shop
	{
		public virtual int ShopId { get; set; }
		public virtual string ShopName { get; set; }
		public virtual District District { get; set; }
	}
}

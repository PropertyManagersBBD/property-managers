namespace Backend.DTOs
{
	public class SaleApprovalDto
	{
		public long PropertyId { get; set; }
		public long BuyerId { get; set; }
		public long SellerId { get; set; }
		public long Price { get; set; }
		public bool Approval { get; set; }

		public Database.Models.SaleContract? ToSaleContract()
		{
			if(!Approval)
				return null;

			return new Database.Models.SaleContract()
			{
				PropertyId = PropertyId,
				BuyerId = BuyerId,
				SellerId = SellerId,
				Price = Price
			};

		}
	}

	public class RentalApprovalDto
	{
		public long PropertyId { get; set; }
		public long LandlordId { get; set; }
		public long TenantId { get; set; }
		public long Price { get; set; }
		public bool IsActive { get; set; }
		public bool Approval { get; set; }

		public Database.Models.RentalContract? ToRentalContract()
		{
			if(!Approval)
				return null;

			return new Database.Models.RentalContract()
			{
				PropertyId = PropertyId,
				LandlordId = LandlordId,
				TenantId = TenantId,
				Rent = Price,
				IsActive = IsActive
			};
		}
	}
}

using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		void SpawnProperties();
		List<Property> GetTop5Properties();
		List<Property> GetProperties(int pageNumber, int pageSize, long? Id, long? OwnerId, int? Capacity);
		List<SaleContract> GetSaleContracts(int pageNumber, int pageSize, long? Id, long? PropertyId, int? Capacity);
		List<RentalContract> GetRentalContracts(int pageNumber, int pageSize, long? Id, long? PropertyId, int? Capacity);
		void SetPrice(long newPrice);
        long GetPrice(int size);
        long GetProperty(int size, bool ToRent);
		long GetPropertyOwner(long propertyId);
		void ListForSale(long Id);
		void ListForRent(long Id);
		bool ApprovePropertySale(SaleApprovalDto approvalDto);
		bool ApprovePropertyRental(RentalApprovalDto approvalDto);
	}
	
}
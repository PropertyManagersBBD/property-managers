using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		void SpawnProperties();
		List<Property> GetTop5Properties();
		List<Property> GetAllProperties(int pageNumber, int pageSize);
		List<SaleContract> GetAllSaleContracts(int pageNumber, int PageSize);
		List<RentalContract> GetAllRentalContracts(int pageNumber, int PageSize);
		void SetPrice(decimal newPrice);
        decimal GetPrice(int size);
        long GetProperty(int size, bool ToRent);
		long GetPropertyOwner(long propertyId);
		void ListForSale(long Id);
		void ListForRent(long Id);
    }
	
}
using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		void SpawnProperties();
		List<Property> GetTop5Properties();
		List<Property> GetAllProperties(int pageNumber);
		List<SaleContract> GetAllSaleContracts(int pageNumber);
		List<RentalContract> GetAllRentalContracts(int pageNumber);
		void SetPrice(decimal newPrice);
        decimal GetPrice(int size);
        long GetProperty(int size, bool ToRent);
		long GetPropertyOwner(long propertyId);
		void ListForSale(long Id);
		void ListForRent(long Id);
    }
	
}
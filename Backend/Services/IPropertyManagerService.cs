using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		void SpawnProperties();
		List<Property> GetTop5Properties();
		List<Property> GetAllProperties(int pageNumber, int pageSize);
		List<Property> GetByPropertiesId(long id, int pageNumber, int pageSize);
		List<Property> GetByPropertiesCapacity(long capacity, int pageNumber, int pageSize);
		List<Property> GetByPropertiesOwnerId(long ownerId, int pageNumber, int pageSize);
		List<Property> GetByPropertiesIdAndOwnerId(long id, long ownerId, int pageNumber, int pageSize);
		List<Property> GetByPropertiesIdAndCapacity(long id, int capacity, int pageNumber, int pageSize);
		List<Property> GetByPropertiesOwnerIdAndCapacity(long ownerId, int capacity, int pageNumber, int pageSize);
		List<Property> GetByPropertiesAllFilter(long id, long ownerId, int capacity, int pageNumber, int pageSize);
		List<SaleContract> GetAllSaleContracts(int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsId(long id, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsCapacity(int capacity, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsPropertyId(long propertyId, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsIdAndPropertyId(long id, long propertyId, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsIdAndCapacity(long id, int capacity, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsPropertyIdAndCapacity(long propertyId, int capacity, int pageNumber, int pageSize);
		List<SaleContract> GetBySaleContractsAllFilter(long id, long propertyId, int capacity, int pageNumber, int pageSize);
		List<RentalContract> GetAllRentalContracts(int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsId(long id, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsCapacity(int capacity, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsPropertyId(long propertyId, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsIdAndPropertyId(long id, long propertyId, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsIdAndCapacity(long id, int capacity, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsPropertyIdAndCapacity(long propertyId, int capacity, int pageNumber, int pageSize);
		List<RentalContract> GetByRentalContractsAllFilter(long id, long propertyId, int capacity, int pageNumber, int pageSize);
		void SetPrice(decimal newPrice);
        decimal GetPrice(int size);
        long GetProperty(int size, bool ToRent);
		long GetPropertyOwner(long propertyId);
		void ListForSale(long Id);
		void ListForRent(long Id);
    }
	
}
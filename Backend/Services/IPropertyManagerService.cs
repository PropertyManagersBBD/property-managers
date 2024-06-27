using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		int SpawnProperties(int num);
		List<Property> GetTop5Properties();
		void SetPrice(decimal newPrice);
		void ListForSale(long Id);
		void ListForRent(long Id);
	}
}
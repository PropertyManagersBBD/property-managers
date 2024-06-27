using Backend.DTOs;

namespace Backend.Services
{
	public interface IPropertyManagerService
	{
		int SpawnProperties(int num);
		List<Property> GetTop5Properties();
		void SetPrice(decimal newPrice);
        decimal GetPrice(int size);
        long GetProperty(int size, bool ToRent);
    }
}
using Backend.DTOs;

namespace Backend.Services
{
	public class PropertyManagerService : IPropertyManagerService
	{
		public void SpawnProperties()
		{
			int numProperties = (new Random().Next() % 10) + 1; // Between 1 and 10
			if(numProperties > 7) // 30% chance
			{
				throw new Exception("Too many properties generated");
			}
		}
	}
}

using Backend.DTOs;

namespace Backend.Services
{
	public class PropertyManagerService : IPropertyManagerService
	{
		public int SpawnProperties(int num)
		{
			int numCreated = 0;
			for (int i=0; i<num; i++)
      {
				int numProperties = (new Random().Next() % 7) + 1; // Between 1 and 8
				Property property = new Property(numProperties);
				numCreated = i + 1;
			}
			return numCreated;
		}
	}
}

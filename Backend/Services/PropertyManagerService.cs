using Backend.Database.Context;
using Backend.DTOs;
using Microsoft.Identity.Client;

namespace Backend.Services
{
	public class PropertyManagerService : IPropertyManagerService
	{
		private readonly PropertyManagerContext _propertyManagerContext;
		private static decimal LatestPricePerUnit { get; set; }

		public PropertyManagerService(PropertyManagerContext propertyManagerContext)
		{
			_propertyManagerContext = propertyManagerContext;
		}

		public int SpawnProperties(int num)
		{
			int numCreated = 0;
			for(int i = 0; i < num; i++)
			{
				int numProperties = (new Random().Next() % 7) + 1; // Between 1 and 8
				Property property = new Property(numProperties);
				numCreated = i + 1;
			}
			return numCreated;
		}


		public List<Property> GetTop5Properties()
		{
			var properties = _propertyManagerContext.Properties.ToList().Take(5);

			var result = properties.Select(prop => new Property(prop.Capacity)
			{
				OwnerId = prop.OwnerId ?? 0
			}).ToList();
			return result;
		}

		public void SetPrice(decimal newPrice)
		{
			if(newPrice < 0)
				throw new Exception("Price of property must be greater than 0");

			LatestPricePerUnit = newPrice;
		}

		public long GetPropertyOwner(long propertyId)
		{
			var dbResult = _propertyManagerContext.Properties.Where(p=>p.Id == propertyId).FirstOrDefault();
			if(dbResult == null)
				throw new Exception($"Property {propertyId} does not exist");

			var ownerId = dbResult.OwnerId ?? -1; // If I leave this comment in the pr, ask em to remove it.
			// The ownerId will be null if the property is not owned, in which case it will be -1. We can either return -1 or see the exception message

			if (ownerId == -1)
				throw new Exception($"Property {propertyId} does not have an owner");

			return ownerId;
		}
		
	}
}

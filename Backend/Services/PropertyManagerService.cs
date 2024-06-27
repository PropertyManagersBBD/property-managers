using Backend.Database.Context;
using Backend.DTOs;
using System.Diagnostics;

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

		public decimal GetPrice(int size)
		{
            return LatestPricePerUnit * size;
		}

		public long GetProperty(int size, bool ToRent)
		{
            long id = _propertyManagerContext.Properties
                .Where(p => p.Capacity == size)
                .Select(p => p.Id)
                .AsEnumerable()
                .DefaultIfEmpty(-1)
                .FirstOrDefault();
            //things left to do:
            //1) set on hold to true
            //2) extend the linq to only accept houses that are rentable/on sale
            Debug.WriteLine(id);
			return id;
        }
	}
}

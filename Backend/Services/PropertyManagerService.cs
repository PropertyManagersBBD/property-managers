using Backend.Database.Context;
using Backend.Database;
using Backend.DTOs;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
	public class PropertyManagerService : IPropertyManagerService
	{
		private readonly PropertyManagerContext _propertyManagerContext;
		private static decimal LatestPricePerUnit { get; set; }

		public PropertyManagerService(PropertyManagerContext propertyManagerContext)
		{
			_propertyManagerContext = propertyManagerContext;
			SpawnProperties();
		}

		public void SpawnProperties()
		{
			if (_propertyManagerContext.Properties.IsNullOrEmpty()){
				var properties = new List<Database.Models.Property>();
				for(int i = 0; i < 50; i++)
				{
					int numProperties = (new Random().Next() % 7) + 1; // Between 1 and 8
					Database.Models.Property property = new Database.Models.Property
					{
						Capacity = numProperties,
						ListedForRent = true,
						ListedForSale = true
					};
					properties.Add(property);
				}
				_propertyManagerContext.Properties.AddRange(properties);
				_propertyManagerContext.SaveChanges();
			}
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
			long id = 0;
			if (ToRent)
			{
			id = _propertyManagerContext.Properties.Where(p => p.Capacity == size && p.ListedForRent==true && p.Pending==false).Select(p => p.Id).AsEnumerable().DefaultIfEmpty(-1).FirstOrDefault();

			}
			else
			{

            id = _propertyManagerContext.Properties.Where(p => p.Capacity == size && p.ListedForSale == true && p.Pending == false).Select(p => p.Id).AsEnumerable().DefaultIfEmpty(-1).FirstOrDefault();
			}

            var entitiy = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == id);

			if(entitiy != null)
			{
				entitiy.Pending = true;
                _propertyManagerContext.SaveChanges();
            }
            Debug.WriteLine(id);
			return id;
        }
        public void ListForRent(long Id)
        {
            var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);

            if (entity != null)
            {
                entity.ListedForRent = true;
                _propertyManagerContext.SaveChanges();
            }
        }

        public void ListForSale(long Id)
		{
			var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);
        
			if (entity != null)
			{
				entity.ListedForSale = true;
				_propertyManagerContext.SaveChanges();
			}	
		}
	}
}

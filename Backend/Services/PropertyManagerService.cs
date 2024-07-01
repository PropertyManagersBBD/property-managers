using System.Runtime.ConstrainedExecution;
using Backend.Database.Context;
using Backend.DTOs;
using Microsoft.Identity.Client;
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
			if(_propertyManagerContext.Properties.IsNullOrEmpty())
			{
				var properties = new List<Database.Models.Property>();
				for(int i = 0; i < 50; i++)
				{
					int numProperties = (new Random().Next() % 7) + 1; // Between 1 and 8
					Database.Models.Property property = new Database.Models.Property
					{
						OwnerId = -1,
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

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();

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
			if(ToRent)
			{
				id = _propertyManagerContext.Properties.Where(p => p.Capacity == size && p.ListedForRent == true && p.Pending == false).Select(p => p.Id).AsEnumerable().DefaultIfEmpty(-1).FirstOrDefault();

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
			return id;
		}

		public long GetPropertyOwner(long propertyId)
		{
			var dbResult = _propertyManagerContext.Properties.Where(p=>p.Id == propertyId).FirstOrDefault();
			if(dbResult == null)
				throw new Exception($"Property {propertyId} does not exist");


			return dbResult.OwnerId;
		}
		
		public void ListForRent(long Id)
		{
			var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);

			if(entity != null)
			{
				entity.ListedForRent = true;
				_propertyManagerContext.SaveChanges();
			}
		}

		public void ListForSale(long Id)
		{
			var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);

			if(entity != null)
			{
				entity.ListedForSale = true;
				_propertyManagerContext.SaveChanges();
			}
		}

		public List<Property> GetAllProperties(int pageNumber, int pageSize) {
			var properties = _propertyManagerContext.Properties.ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}

		public List<Property> GetByPropertiesId(long id, int pageNumber, int pageSize) {
			var properties = _propertyManagerContext.Properties.Where(x => x.Id == id).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}

		public List<Property> GetByPropertiesCapacity(long capacity, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.Capacity == capacity).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}
		public List<Property> GetByPropertiesOwnerId(long ownerId, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.OwnerId == ownerId).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}

		public List<Property> GetByPropertiesIdAndOwnerId(long id, long ownerId, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.OwnerId == ownerId).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}
		public List<Property> GetByPropertiesIdAndCapacity(long id, int capacity, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}
		public List<Property> GetByPropertiesOwnerIdAndCapacity(long ownerId, int capacity, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.OwnerId == ownerId && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}
		public List<Property> GetByPropertiesAllFilter(long id, long ownerId, int capacity, int pageNumber, int pageSize){
			var properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.OwnerId == ownerId && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}

		public List<SaleContract> GetAllSaleContracts(int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price}).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsId(long id, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.Id == id).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsCapacity(int capacity, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsPropertyId(long propertyId, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.PropertyId == propertyId).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsIdAndPropertyId(long id, long propertyId, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.Id == id && x.PropertyId == propertyId).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsIdAndCapacity(long id, int capacity, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.Id == id && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsPropertyIdAndCapacity(long propertyId, int capacity, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.PropertyId == propertyId && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<SaleContract> GetBySaleContractsAllFilter(long id, long propertyId, int capacity, int pageNumber, int pageSize){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
			.Where(x => x.Id == id && x.PropertyId == propertyId && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<RentalContract> GetAllRentalContracts(int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive}).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsId(long id, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.Id == id).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsCapacity(int capacity, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsPropertyId(long propertyId, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.PropertyId == propertyId).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsIdAndPropertyId(long id, long propertyId, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.Id == id && x.PropertyId == propertyId).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsIdAndCapacity(long id, int capacity, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.Id == id && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsPropertyIdAndCapacity(long propertyId, int capacity, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.PropertyId == propertyId && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
		public List<RentalContract> GetByRentalContractsAllFilter(long id, long propertyId, int capacity, int pageNumber, int pageSize){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
			.Where(x => x.Id == id && x.PropertyId == propertyId && x.Capacity == capacity).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}
	}
}

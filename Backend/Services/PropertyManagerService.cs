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
		private static long LatestPricePerUnit { get; set; }

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

		public void SetPrice(long newPrice)
		{
			if(newPrice < 0)
				throw new Exception("Price of property must be greater than 0");

			LatestPricePerUnit = newPrice;
		}

		public long GetPrice(int size)
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
		
		public String ListForRent(long Id)
		{
			var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);

			if(entity != null)
			{
				entity.ListedForRent = true;
				_propertyManagerContext.SaveChanges();
				return "Proprty " + Id + " has been listed for rent";
			} else {
				return "Proprty " + Id + " does not exist";
			}
		}

		public String ListForSale(long Id)
		{
			var entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == Id);

			if(entity != null)
			{
				entity.ListedForSale = true;
				_propertyManagerContext.SaveChanges();
				return "Proprty " + Id + " has been listed for sale";
			} else {
				return "Proprty " + Id + " does not exist";
			}
		}

		public List<Property> GetProperties(int pageNumber, int pageSize, long? id, long? ownerId, int? capacity) {
			var properties = _propertyManagerContext.Properties.ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			if(id != null && ownerId != null && capacity != null){
				properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.OwnerId == ownerId && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null && ownerId != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.OwnerId == ownerId).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null && capacity != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.Id == id && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (ownerId != null && capacity != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.OwnerId == ownerId && x.Capacity == capacity).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.Id == id).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (ownerId != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.OwnerId == ownerId).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (capacity != null) {
				properties = _propertyManagerContext.Properties.Where(x => x.Capacity == capacity).ToList().OrderBy(x => x.Id)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			}
			

			var result = properties.Select(prop => new Property(prop.Id, prop.OwnerId, prop.Capacity, prop.ListedForSale, prop.ListedForRent, prop.Pending)).ToList();
			
			return result;
		}

		public List<SaleContract> GetSaleContracts(int pageNumber, int pageSize, long? id, long? propertyId, int? capacity){
			var saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price}).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			if (id != null && propertyId != null && capacity != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.Id == id && x.PropertyId == propertyId && x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null && propertyId != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.Id == id && x.PropertyId == propertyId).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null && capacity != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.Id == id && x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (propertyId != null && capacity != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.PropertyId == propertyId && x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.Id == id).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (propertyId != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.PropertyId == propertyId).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (capacity != null) {
				saleContracts = (from p in _propertyManagerContext.Properties join s in _propertyManagerContext.SaleContracts on p.Id equals s.PropertyId select new {s.Id, s.PropertyId, s.SellerId, s.BuyerId, p.Capacity, s.Price})
				.Where(x => x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			}

			var result = saleContracts.Select(s => new SaleContract(s.Id, s.PropertyId, s.SellerId, s.BuyerId, s.Capacity, s.Price)).ToList();
			
			return result;
		}
		public List<RentalContract> GetRentalContracts(int pageNumber, int pageSize, long? id, long? propertyId, int? capacity){
			var rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive}).ToList()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();

			if (id != null && propertyId != null && capacity != null) {

			} else if (id != null && propertyId != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.Id == id && x.PropertyId == propertyId).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null && capacity != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.Id == id && x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (propertyId != null && capacity != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.PropertyId == propertyId && x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (id != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.Id == id).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (propertyId != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.PropertyId == propertyId).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			} else if (capacity != null) {
				rentalContracts = (from p in _propertyManagerContext.Properties join r in _propertyManagerContext.RentalContracts on p.Id equals r.PropertyId select new {r.Id, r.PropertyId, r.LandlordId, r.TenantId, p.Capacity, r.Rent, r.IsActive})
				.Where(x => x.Capacity == capacity).ToList()
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			}

			var result = rentalContracts.Select(r => new RentalContract(r.Id, r.PropertyId, r.LandlordId, r.TenantId, r.Capacity, r.Rent, r.IsActive)).ToList();
			
			return result;
		}

		public List<PropertySummary> GetPropertiesByOwners(long[] ownerIds) {
			var properties = _propertyManagerContext.Properties.Where(x => ownerIds.Contains(x.OwnerId)).OrderBy(x => x.Id)
				.ToList();

			var result = properties.Select(prop => new PropertySummary(prop.Id, prop.OwnerId, prop.Capacity, GetPrice(prop.Capacity))).ToList();
			
			return result;
		}

		public void DailyUpdate(Deaths[] deaths) {
			var properties = new List<PropertySummary>();
			var entity = new Database.Models.Property();
			foreach (var death in deaths){
				properties = GetPropertiesByOwners([death.deceased]);
				foreach (var property in properties){
					Database.Models.SaleContract saleContract = new Database.Models.SaleContract{ PropertyId = property.Id ?? -1, BuyerId = death.next_of_kin, SellerId = death.deceased, Price = 0};
					_propertyManagerContext.SaleContracts.Add(saleContract);
					entity = _propertyManagerContext.Properties.FirstOrDefault(item => item.Id == property.Id);
					if(entity != null)
					{
						entity.OwnerId = death.next_of_kin;
					}
				}
			}
			try {
				_propertyManagerContext.SaveChanges();
			} catch(Exception e) {
				throw new Exception(e.Message);
			}
		}		
		public bool ApprovePropertySale(SaleApprovalDto approvalDto)
		{
			if(!approvalDto.Approval) return false;

			var saleContract = approvalDto.ToSaleContract();
			var property = _propertyManagerContext.Properties.Where(p => p.Id == approvalDto.PropertyId).FirstOrDefault();

			if ((saleContract == null) || (property==null)) return false;
			if (saleContract.SellerId != property.OwnerId) return false;

			_propertyManagerContext.SaleContracts.Add(saleContract);
			property.OwnerId = approvalDto.BuyerId;
			property.Pending = false;

			_propertyManagerContext.SaveChanges();

			return true;
		}

		public bool ApprovePropertyRental(RentalApprovalDto approvalDto)
		{
			if(!approvalDto.Approval) return false;

			var contract = approvalDto.ToRentalContract();
			var previousActiveContract = _propertyManagerContext.RentalContracts.Where(rc => rc.PropertyId == approvalDto.PropertyId && !rc.IsActive).FirstOrDefault();
			if (previousActiveContract!=null) previousActiveContract.IsActive = false;

			var property = _propertyManagerContext.Properties.Where(p => p.Id == approvalDto.PropertyId).FirstOrDefault();

			if (contract == null || property==null) return false;
			if(contract.LandlordId != property.OwnerId) return false;

			_propertyManagerContext.RentalContracts.Add(contract);
			property.ListedForRent = contract.IsActive;
			property.Pending = false;

			_propertyManagerContext.SaveChanges();

			return true;
		}

	}
}

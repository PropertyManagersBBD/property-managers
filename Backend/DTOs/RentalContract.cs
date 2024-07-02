using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public class RentalContract
  {
    public long? Id { get; set; }
    public long PropertyId { get; set; }
    public long LandlordId { get; set; }
    public long TenantId { get; set; }
    public int Capacity { get; set; }
    public long Price { get; set; }
    public bool IsActive { get; set; }

    public RentalContract() { }

    public RentalContract(long id, long propertyId, long landlordId, long tenantId,int capacity, long price, bool isActive)
    {
      Id = id;
      PropertyId = propertyId;
      LandlordId = landlordId;
      TenantId = tenantId;
      Capacity = capacity;
      Price = price;
      IsActive = isActive;
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public class PropertySummary
  {
    public long? Id { get; set; }
    public long OwnerId { get; set; }

    public int Capacity { get; set; }
    
    public long Price { get; set; }

    public PropertySummary(long id, long ownerId, int capacity, long price)
    {
      Id = id;
      Capacity = capacity;
      OwnerId = ownerId;
      Price = price;
    }
  }
}
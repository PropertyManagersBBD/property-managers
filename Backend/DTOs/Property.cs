using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public class Property
  {
    public long? Id { get; set; }
    public long OwnerId { get; set; }

    public int Capacity { get; set; }

    public bool ListedForRent { get; set; } = false;

    public bool ListedForSale { get; set; } = false;
    
    public bool Pending { get; set; } = false;

    public Property(int capacity)
    {
      Capacity = capacity;
      ListedForRent = true;
      ListedForSale = true;
      Pending = false;
    }

    public Property(long id, long ownerId, int capacity, bool listedForSale, bool listedForRent, bool pending)
    {
      Id = id;
      Capacity = capacity;
      ListedForRent = listedForRent;
      ListedForSale = listedForSale;
      OwnerId = ownerId;
      Pending = pending;
    }
  }
}

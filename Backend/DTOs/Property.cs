using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public class Property
  {
    public long OwnerId { get; set; }

    public int Capacity { get; set; }

    public bool ListedForRent { get; set; } = false;

    public bool ListedForSale { get; set; } = false;

    public Property(int capacity)
    {
      Capacity = capacity;
      ListedForRent = true;
      ListedForSale = true;
    }

    public Property(long ownerId,int capacity, bool listedForSale, bool listedForRent)
    {
      Capacity = capacity;
      ListedForRent = listedForRent;
      ListedForSale = listedForSale;
      OwnerId = ownerId;
    }
  }
}

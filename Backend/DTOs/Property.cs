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

    public Property(int capacity)
    {
      Capacity = capacity;
    }
  }
}

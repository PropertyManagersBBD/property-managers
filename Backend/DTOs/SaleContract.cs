using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public class SaleContract
  {
    public long? Id { get; set; }
    public long PropertyId { get; set; }
    public long SellerId { get; set; }
    public long BuyerId { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }

    public SaleContract(long id, long propertyId, long sellerId, long buyerId,int capacity, decimal price)
    {
      Id = id;
      PropertyId = propertyId;
      SellerId = sellerId;
      BuyerId = buyerId;
      Capacity = capacity;
      Price = price;
    }
  }
}
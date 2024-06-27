namespace Backend.DTOs
{
    public class RequestProperty
    {
        public int size { get; set; }
    }

    public class PropertyResponse
    {
        public decimal Price { get; set; }
        public long PropertyId { get; set; }

        public PropertyResponse(decimal price, long propertyId)
        {
            Price = price;
            PropertyId = propertyId;
        }
    }
}

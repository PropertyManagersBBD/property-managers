namespace Backend.DTOs
{
    public class RequestProperty
    {
        public int size { get; set; }
        public bool toRent {  get; set; }
    }

    public class PropertyResponse
    {
        public long Price { get; set; }
        public long PropertyId { get; set; }

        public PropertyResponse(long price, long propertyId)
        {
            Price = price;
            PropertyId = propertyId;
        }
    }
}

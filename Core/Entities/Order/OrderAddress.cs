using Core.Enums;

namespace Core.Entities.Order
{
    public class OrderAddress
    {
        public int Id { get; set; }
        public string DeliveryName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }

    }
}
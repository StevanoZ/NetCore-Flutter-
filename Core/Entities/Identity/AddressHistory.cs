using Core.Enums;

namespace Core.Entities.Identity
{
    public class AddressHistory
    {
        public int Id { get; set; }
        public string DeliveryName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

    }
}
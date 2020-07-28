using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class AddressHistoryDto
    {
        [Required]
        public string DeliveryName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [Required]
        public long PhoneNumber { get; set; }

    }
}
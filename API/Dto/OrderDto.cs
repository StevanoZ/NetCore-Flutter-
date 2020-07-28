using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public AddressHistoryDto Address { get; set; }

        [Required]
        public string DeliveryMethod { get; set; }
        public double DeliveryCost { get; set; }
    }
}
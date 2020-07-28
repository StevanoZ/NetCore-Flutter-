namespace API.Dto
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public int Quantity { get; set; }
    }
}
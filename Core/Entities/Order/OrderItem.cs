namespace Core.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, double price, double netPrice, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            NetPrice = netPrice;
            Quantity = quantity;
        }
        public ProductItemOrdered ItemOrdered { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public int Quantity { get; set; }
    }
}
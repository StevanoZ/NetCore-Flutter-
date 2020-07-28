using System.Collections.Generic;

namespace Core.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public CustomerBasket()
        {
           
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
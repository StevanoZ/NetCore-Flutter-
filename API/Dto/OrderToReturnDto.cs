using System;
using System.Collections.Generic;
using Core.Entities.Order;

namespace API.Dto
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderAddress ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public double DeliveryCost { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }
}
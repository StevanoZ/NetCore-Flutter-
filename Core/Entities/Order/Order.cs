using System;
using System.Collections.Generic;
using Core.Entities.Identity;
using Core.Enums;

namespace Core.Entities.Order
{
    public class Order : BaseEntity
    {

        public Order()
        {

        }
        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, double subTotal, double deliveryCost, OrderAddress shipToAddress, PaymentMethod paymentMethod, OrderStatus status)
        {
            OrderItems = orderItems;
            SubTotal = subTotal;
            DeliveryCost = deliveryCost;
            ShipToAddress = shipToAddress;
            PaymentMethod = paymentMethod;
            Status = status;
            BuyerEmail = buyerEmail;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public double SubTotal { get; set; }
        public double DeliveryCost { get; set; }
        public OrderAddress ShipToAddress { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string PaymentOrderUrl { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public double GetTotal()
        {
            return SubTotal + DeliveryCost;
        }

    }
}
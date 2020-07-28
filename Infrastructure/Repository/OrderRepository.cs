using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basket;
        private readonly IPhotoRepository _photo;
        public OrderRepository(IUnitOfWork unitOfWork, IBasketRepository basket, IPhotoRepository photo)
        {
            _photo = photo;
            _basket = basket;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string paymentMethod, double deliveryCost, string basketId, OrderAddress address)
        {
            var basket = await _basket.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            var inValidProduct = false;
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Quantity > productItem.Quantity)
                {
                    inValidProduct = true;
                    break;

                }
                if (!item.IsActive)
                {
                    inValidProduct = true;
                    break;
                }
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.Photos.FirstOrDefault(p => p.IsMain)?.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, productItem.NetPrice, item.Quantity);
                items.Add(orderItem);
                productItem.Quantity -= item.Quantity;
                _unitOfWork.Repository<Product>().Update(productItem);
            }
            if (!inValidProduct)
            {
                var subTotal = items.Sum(it => it.Price * it.Quantity);
                var order = new Order(items, buyerEmail, subTotal, deliveryCost, address,
                 paymentMethod == "COD" ? PaymentMethod.COD : paymentMethod == "COD**" ? PaymentMethod.COD_ : PaymentMethod.BankTransfer,
                 paymentMethod == "COD" ? OrderStatus.WaitingConfirmation : paymentMethod == "COD**" ? OrderStatus.WaitingConfirmation : OrderStatus.WaitingTransfer);
                _unitOfWork.Repository<Order>().Add(order);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return null;

                return order;
            }

            return null;

        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAllWithSpecAsync(spec);
        }

        public async Task<Order> UpdateOrderByAdmin(int id, string buyerEmail, string status)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            order.Status = status == "Pending" ? OrderStatus.Pending :
            status == "On Process" ? OrderStatus.OnProcess :
            status == "Failed" ? OrderStatus.Failed : OrderStatus.Success;
            _unitOfWork.Repository<Order>().Update(order);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;
        }

        public async Task<Order> UpdateOrderPaymentUrl(int id, string buyerEmail, IFormFile file)
        {
            if (file.Length > 0)
            {
                var photo = await _photo.SaveToDiskAsync(file);
                var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
                var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
                order.PaymentOrderUrl = photo.PictureUrl;
                _unitOfWork.Repository<Order>().Update(order);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return null;

                return order;
            }
            return null;
        }
    }
}
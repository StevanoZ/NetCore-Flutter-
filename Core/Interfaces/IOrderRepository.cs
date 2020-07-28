using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Order;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string paymentMethod, double deliveryCost, string basketId, OrderAddress address);
        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

        Task<Order> UpdateOrderPaymentUrl(int id, string buyerEmail, IFormFile file);
        Task<Order> UpdateOrderByAdmin(int id, string buyerEmail, string status);
    }
}
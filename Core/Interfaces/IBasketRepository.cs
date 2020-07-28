using System.Threading.Tasks;
using Core.Entities.Basket;

namespace Core.Interfaces {
    public interface IBasketRepository {
        Task<CustomerBasket> GetBasketAsync (string id);
        Task<CustomerBasket> SetBasketAsync (CustomerBasket basket);
        Task<bool> DeleteBasketAsync (string id);
    }
}
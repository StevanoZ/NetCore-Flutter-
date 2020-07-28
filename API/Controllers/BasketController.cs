using System.Threading.Tasks;
using API.Dto;
using AutoMapper;
using Core.Entities.Basket;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class BasketController : BaseApIController {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        public BasketController (IBasketRepository basketRepo, IMapper mapper) {
            _mapper = mapper;
            _basketRepo = basketRepo;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket (string id) {
            var basket = await _basketRepo.GetBasketAsync (id);
            return Ok (basket ?? new CustomerBasket (id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket (CustomerBasketDto customerBasketDto) {
            var basket = _mapper.Map<CustomerBasket> (customerBasketDto);
            var updatedBasket = await _basketRepo.SetBasketAsync (basket);
            return Ok (basket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync (string id) {
            await _basketRepo.DeleteBasketAsync (id);
        }

    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Order;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApIController
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrdersController(IOrderRepository orderRepo, IMapper mapper)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;

        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<OrderAddress>(orderDto.Address);
            var order = await _orderRepo.CreateOrderAsync(email, orderDto.DeliveryMethod, orderDto.DeliveryCost, orderDto.BasketId, address);
            if (order == null) return BadRequest(new ApiResponse(400, "Invalid creating order, maybe because your order quantity not valid or your order contains not active product"));
            return order;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(string buyerEmail)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var orders = await _orderRepo.GetOrderForUserAsync(buyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id, string buyerEmail)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var order = await _orderRepo.GetOrderByIdAsync(id, buyerEmail);
            if (order == null) return NotFound(new ApiResponse(404, "Order not found"));

            return _mapper.Map<OrderToReturnDto>(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("order/{id}/admin")]
        public async Task<ActionResult<OrderToReturnDto>> UpdateOrderAdmin(int id, string buyerEmail, string status)
        {
            var updatedOrder = await _orderRepo.UpdateOrderByAdmin(id, buyerEmail, status);
            if (updatedOrder == null) return BadRequest(new ApiResponse(400, "Failed updating order"));

            return Ok(_mapper.Map<OrderToReturnDto>(updatedOrder));
        }

        [HttpPut("order/{id}/member")]
        public async Task<ActionResult<OrderToReturnDto>> UpdateOrderMember(int id, string buyerEmail, [FromForm] PhotoUploadDto photoUpload)
        {
            var updatedOrder = await _orderRepo.UpdateOrderPaymentUrl(id, buyerEmail, photoUpload.Photo);
            if (updatedOrder == null) return BadRequest(new ApiResponse(400, "Failed updating order"));

            return Ok(_mapper.Map<OrderToReturnDto>(updatedOrder));
        }
    }
}
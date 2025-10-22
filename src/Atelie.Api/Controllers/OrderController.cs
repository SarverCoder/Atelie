using Atelie.Application.Models.Orders;
using Atelie.Application.Services;
using Atelie.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        return Ok(await _orderService.CreateOrderAsync(dto));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetOrderById(long id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrderStatus(long id, OrderStatus status)
    {
        return Ok(await _orderService.UpdateOrderStatus(id, status));
    }

    [HttpPatch("delete/{id:long}")]
    public async Task<IActionResult> SetDeleteStatus(long id, bool isDeleted)
    {
        var result = await _orderService.SetDeleteStatusAsync(id, isDeleted);
        return Ok(result);
    }

}

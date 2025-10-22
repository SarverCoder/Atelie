using Atelie.Application.Models;
using Atelie.Application.Models.Orders;
using Atelie.Domain.Enums;

namespace Atelie.Application.Services;

public interface IOrderService
{
    Task<ApiResult<string>> CreateOrderAsync(CreateOrderDto dto);
    Task<OrderDto> GetOrderByIdAsync(long id);
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<ApiResult<string>> UpdateOrderStatus(long id, OrderStatus status);
    Task<ApiResult<string>> SetDeleteStatusAsync(long id, bool isDeleted);
}       
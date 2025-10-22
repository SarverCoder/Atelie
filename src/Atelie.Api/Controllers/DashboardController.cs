using Atelie.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]    
    public async Task<IActionResult> GetDashboardSummary([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] int take = 10)
    {
        var summary = await _dashboardService.GetAsync(from, to, take);
        return Ok(summary);
    }

    [HttpGet("recent-orders")]
    public async Task<IActionResult> GetRecentOrders()
    {
        var orders = await _dashboardService.GetRecentOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("status-count")]
    public async Task<IActionResult> GetStatusCount([FromQuery] Domain.Enums.OrderStatus status)
    {
        var statusCount = await _dashboardService.GetStatusCountsAsync(status);
        return Ok(statusCount);
    }
}

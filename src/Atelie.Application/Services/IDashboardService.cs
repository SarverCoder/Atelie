using Atelie.Application.Models.Dashboards;
using Atelie.Domain.Enums;

namespace Atelie.Application.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetAsync(DateTime? from = null, DateTime? to = null, int recentTake = 10);
    Task<List<DashboardLastOrderDto>> GetRecentOrdersAsync();
    Task<DashboardStatusDto> GetStatusCountsAsync(OrderStatus status);
}
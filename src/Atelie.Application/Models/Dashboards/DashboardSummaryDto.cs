namespace Atelie.Application.Models.Dashboards;

public class DashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal? TotalOrdersMoM { get; set; }   // % o'zgarish (oyma-oy)

    public int ActiveOrders { get; set; }
    public decimal? ActiveOrdersMoM { get; set; }

    public int ReadyOrders { get; set; }
    public decimal? ReadyOrdersMoM { get; set; }

    public decimal TotalRevenue { get; set; }
    public decimal? TotalRevenueMoM { get; set; }
}
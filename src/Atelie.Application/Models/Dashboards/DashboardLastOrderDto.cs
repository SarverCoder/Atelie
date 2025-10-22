namespace Atelie.Application.Models.Dashboards;

public class DashboardLastOrderDto
{
    public string OrderNumber { get; set; }
    public string CustomerName { get; set; }
    public string ItemsShort { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
}       
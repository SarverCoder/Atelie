using Atelie.Domain.Common;

namespace Atelie.Domain.Entities;

public class Order : Auditable  
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string OrderNumber { get; set; } = default!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }

    public string? Notes { get; set; }

    // Har bir buyurtmada bir nechta xodim (manager/usta/bichuvchi...) bo‘ladi

    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public decimal PaidAmount { get; set; }


    public ICollection<OrderWorker> Workers { get; set; } = new List<OrderWorker>();

}
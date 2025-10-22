using Atelie.Domain.Common;
using Atelie.Domain.Enums;

namespace Atelie.Domain.Entities;

public class Order : Auditable  
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string OrderNumber { get; set; } 

    public string? Color { get; set; }
    public string? LiningType { get; set; }
    public string? ButtonType { get; set; }


    public decimal? ОГ { get; set; }
    public decimal? ОТ { get; set; }
    public decimal? ОБ { get; set; }
    public decimal? ШГ { get; set; }
    public decimal? ШС { get; set; }
    public decimal? ДТС { get; set; }
    public decimal? ДТП2 { get; set; }
    public decimal? ДЛ { get; set; }
    public decimal? ПЛ { get; set; }
    public decimal? Ру { get; set; }
    public decimal? Шбс { get; set; }
    public decimal? Шн { get; set; }
    public decimal? ДГ { get; set; }



    public decimal AdvancePayment { get; set; } 
    public decimal RemainingAmount { get; set; }    
    public decimal BasePrice { get; set; }
    public decimal? AdditionalCost { get; set; }

    public string? SpecialRequests { get; set; }
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; } = false;
        
    public OrderStatus Status { get; set; } = OrderStatus.Yangi;
    public DateTime? CompletedDate { get; set; }


    // Bir necha ishchi bir buyurtmaga
    public ICollection<OrderWorker> OrderWorkers { get; set; } = new List<OrderWorker>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
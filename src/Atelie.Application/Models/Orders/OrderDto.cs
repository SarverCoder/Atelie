using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Orders;

public class OrderDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string OrderNumber { get; set; } = default!;

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

    public decimal BasePrice { get; set; }
    public decimal? AdditionalCost { get; set; }
    public decimal AdvancePayment { get; set; }
    public decimal RemainingAmount { get; set; }

    public string? SpecialRequests { get; set; }
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; } 

    public List<OrderItemDto> Items { get; set; } = new();
    public List<OrderWorkerDto> Workers { get; set; } = new();
}

public class OrderItemDto
{
    public long Id { get; set; }
    public string OrderType { get; set; }
    public long? FabricId { get; set; }
    public string? FabricName { get; set; } // agar Fabric nomi kerak bo‘lsa
    public decimal? FabricQuantity { get; set; }   // masalan: 2.3 (metr)
}

public class OrderWorkerDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? UserFullName { get; set; } // qulaylik uchun
    public decimal Cost { get; set; }
    public string WorkerRole { get; set; }
    public DateTime AssignedAt { get; set; }
}
using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Orders;

public class CreateOrderDto
{
    public long CustomerId { get; set; }

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
    public decimal BasePrice { get; set; }
    public decimal? AdditionalCost { get; set; }

    public string? SpecialRequests { get; set; }
    public string? Notes { get; set; }

    public DateTime? CompletedDate { get; set; }


    public List<CreateOrderItemDto> Items { get; set; }  
    public List<CreateOrderWorker> Workers { get; set; }    


}

public class CreateOrderItemDto
{
    public OrderClothes OrderType { get; set; }   // Kostyum, Shim, Jilet, Ko‘ylak
    public long? FabricId { get; set; }
    public decimal? FabricQuantity { get; set; }   // masalan: 2.3 (metr)
}

public class CreateOrderWorker
{
    public long UserId { get; set; }
    public decimal Cost { get; set; }
    public WorkerRole WorkerRole { get; set; } // Masalan: Tikuvchi, O‘lchovchi, Dizayner
}
using Atelie.Domain.Common;
using Atelie.Domain.Enums;

namespace Atelie.Domain.Entities;

public class OrderItem : BaseClass  
{
    public long OrderId { get; set; }
    public Order Order { get; set; }

    public OrderClothes OrderType { get; set; }   // Kostyum, Shim, Jilet, Ko‘ylak
    public long? FabricId { get; set; }
    public Fabric? Fabric { get; set; }

    public decimal? FabricQuantity { get; set; }
}
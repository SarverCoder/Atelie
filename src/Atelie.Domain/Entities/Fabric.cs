using Atelie.Domain.Common;

namespace Atelie.Domain.Entities;

public class Fabric : Auditable
{
    public string Name { get; set; }          // Masalan: "Italiya matosi", "Turkiya matosi"
    public string? Description { get; set; }  // Qo‘shimcha info
    public decimal BasePrice { get; set; }    // Narxi (asosiy narx)
    public string? ColorOptions { get; set; } // Rang variantlari (masalan: "Qora, Ko‘k, Kulrang")
    public bool IsActive { get; set; } = true;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
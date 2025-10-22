using Atelie.Domain.Common;
using Atelie.Domain.Enums;

namespace Atelie.Domain.Entities;

public class OrderWorker : BaseClass
{
    public long OrderId { get; set; }
    public Order Order { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public decimal Cost { get; set; }   

    public WorkerRole WorkerRole { get; set; } // Masalan: Tikuvchi, O‘lchovchi, Dizayner
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
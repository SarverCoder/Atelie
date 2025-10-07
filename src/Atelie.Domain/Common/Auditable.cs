namespace Atelie.Domain.Common;

public class Auditable : BaseClass
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
}
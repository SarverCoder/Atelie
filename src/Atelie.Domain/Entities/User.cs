using Atelie.Domain.Common;
using Atelie.Domain.Enums;

namespace Atelie.Domain.Entities;

public class User : Auditable
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }   
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiryTime { get; set; }
    public UserRole UserRole { get; set; }  
    public bool IsActive { get; set; } = true;

    public ICollection<OrderWorker> OrderWorkers { get; set; } = new List<OrderWorker>();
}
using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Auths;

public class LoginResponseDto
{
    public long Id { get; set; }    
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string UserRole { get; set; }
    public bool IsActive { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}   
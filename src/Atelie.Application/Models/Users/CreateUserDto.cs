using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Users;

public class CreateUserDto
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Password { get; set; }
    public UserRole UserRole { get; set; }
}
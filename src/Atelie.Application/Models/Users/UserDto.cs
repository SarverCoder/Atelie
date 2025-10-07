using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Users;

public class UserDto
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }   
    public UserRole UserRole { get; set; }
    public bool IsActive { get; set; }
}
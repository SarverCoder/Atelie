using Atelie.Domain.Enums;

namespace Atelie.Application.Models.Customers;

public class CreateCustomerDto
{
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }  
    public CustomerStatus CustomerStatus { get; set; }
    public string? TelegramUsername { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string? ContactTime { get; set; }
}
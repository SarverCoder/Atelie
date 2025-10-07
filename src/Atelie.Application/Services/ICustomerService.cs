using Atelie.Application.Models;
using Atelie.Application.Models.Customers;

namespace Atelie.Application.Services;

public interface ICustomerService
{
    Task<ApiResult<string>> CreateCustomerAsync(CreateCustomerDto dto);
    Task<CustomerDto> GetByIdCustomerAsync(long id);
    Task<List<CustomerDto>> GetAllCustomersAsync();
    Task<ApiResult<string>> UpdateCustomerAsync(long id, UpdateCustomerDto dto);
    Task<ApiResult<string>> DeleteCustomerAsync(long id);
}
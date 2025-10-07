using Atelie.Application.Models.Customers;
using Atelie.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
    {
        var result = await _customerService.CreateCustomerAsync(dto);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCustomer(long id)
    {
        return Ok(await _customerService.GetByIdCustomerAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        return Ok(await _customerService.GetAllCustomersAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(long id, [FromBody] UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateCustomerAsync(id, dto);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(long id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

}

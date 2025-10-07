using Atelie.Application.Models.Users;
using Atelie.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        return Ok(await _userService.CreateUserAsync(dto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByUserId(long id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(long id, UpdateUserDto dto)
    {
        return Ok(await _userService.UpdateUserAsync(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById(long id)
    {
        return Ok(await _userService.DeleteUserAsync(id));
    }

}

using Atelie.Application.Models;
using Atelie.Application.Models.Users;

namespace Atelie.Application.Services;

public interface IUserService
{
    Task<ApiResult<string>> CreateUserAsync(CreateUserDto dto);
    Task<UserDto> GetUserByIdAsync(long id);    
    Task<List<UserDto>> GetUsersAsync();
    Task<ApiResult<string>> UpdateUserAsync(long id, UpdateUserDto dto);
    Task<ApiResult<string>> DeleteUserAsync(long id);
}
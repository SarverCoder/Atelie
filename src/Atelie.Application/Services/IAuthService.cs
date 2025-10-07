using Atelie.Application.Models;
using Atelie.Application.Models.Auths;
using Atelie.Application.Models.Users;

namespace Atelie.Application.Services;

public interface IAuthService
{
    Task<ApiResult<string>> RegisterAsync(CreateUserDto dto);
    Task<ApiResult<LoginResponseDto>> LoginAsync(LoginDto dto);
    Task<ApiResult<TokenDto>> RefreshTokenAsync(string refreshToken);
    Task<ApiResult<string>> ChangePasswordAsync(ChangePasswordDto dto);
    Task<ApiResult<UserDto>> GetCurrentUserAsync();
}
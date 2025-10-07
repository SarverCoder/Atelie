using Atelie.Application.Helpers.GenerateJwt;
using Atelie.Application.Helpers.PasswordHasher;
using Atelie.Application.Models;
using Atelie.Application.Models.Auths;
using Atelie.Application.Models.Users;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Atelie.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        DatabaseContext context,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenHandler jwtTokenHandler,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtTokenHandler = jwtTokenHandler;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResult<string>> RegisterAsync(CreateUserDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == dto.UserName || x.PhoneNumber == dto.PhoneNumber);


        if (existingUser != null)
        {
            return ApiResult<string>.Failure(["Bu username yoki telefon raqami allaqachon ro'yxatdan o'tgan"]);
        }

        var salt = Guid.NewGuid().ToString();
        var hash = _passwordHasher.Encrypt(dto.Password, salt);

        var user = new User
        {
            FullName = dto.FullName,
            UserName = dto.UserName,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = hash,
            PasswordSalt = salt,
            DateOfBirth = dto.DateOfBirth,
            UserRole = dto.UserRole
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("User ro'yxatdan otdi.");

    }

    public async Task<ApiResult<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == dto.UserName);

        if (user is null)
        {
            return ApiResult<LoginResponseDto>.Failure(["Username or password wrong"]);
        }

        if (!_passwordHasher.Verify(user.PasswordHash, dto.Password, user.PasswordSalt))
        {
            return ApiResult<LoginResponseDto>.Failure(["Username or Password wrong"]);
        }

        var accessToken = _jwtTokenHandler.GenerateAccessToken(user, Guid.NewGuid().ToString());
        var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

        // 🔴 MUHIM: refresh tokenni va muddatini saqlaymiz
        user.RefreshToken = refreshToken;
        user.TokenExpiryTime = DateTime.UtcNow.AddDays(7); // yoki konfiguratsiyadan oling

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return ApiResult<LoginResponseDto>.Success(new LoginResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            UserRole = user.UserRole.ToString(),
            IsActive = user.IsActive,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });

    }

    public async Task<ApiResult<TokenDto>> RefreshTokenAsync(string refreshToken)
    {
        // 1. RefreshToken bo‘yicha userni topamiz
        var employee = await _context.Users
            .FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);

        if (employee is null)
            return ApiResult<TokenDto>.Failure(["Invalid refresh token"]);

        // 2. Refresh token muddati tugaganmi?
        if (!employee.TokenExpiryTime.HasValue || employee.TokenExpiryTime < DateTime.UtcNow)
            return ApiResult<TokenDto>.Failure(["Refresh token has expired"]);

        // 3. Yangi access token va refresh token yaratamiz
        var newAccessToken = _jwtTokenHandler.GenerateAccessToken(employee, Guid.NewGuid().ToString());
        var newRefreshToken = _jwtTokenHandler.GenerateRefreshToken();

        // 4. Employee'ni yangilaymiz
        employee.RefreshToken = newRefreshToken;
        employee.TokenExpiryTime = DateTime.UtcNow.AddDays(7); // masalan 7 kun amal qiladi
        await _context.SaveChangesAsync();

        // 5. Response qaytaramiz
        var result = new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return ApiResult<TokenDto>.Success(result);
    }

    public async Task<ApiResult<string>> ChangePasswordAsync(ChangePasswordDto dto)
    {
        // 1. Foydalanuvchini topamiz
        var employee = await _context.Users
            .FirstOrDefaultAsync(e => e.UserName == dto.UserName);

        if (employee is null)
            return ApiResult<string>.Failure(["User not found"]);

        // 2. Eski parolni tekshiramiz
        var oldPasswordHash = _passwordHasher.Encrypt(dto.OldPassword, employee.PasswordSalt);
        if (oldPasswordHash != employee.PasswordHash)
            return ApiResult<string>.Failure(["Old password is incorrect"]);

        // 3. Yangi salt va hash yaratamiz
        var newSalt = Guid.NewGuid().ToString();
        var newPasswordHash = _passwordHasher.Encrypt(dto.NewPassword, newSalt);

        // 4. Employee ma’lumotlarini yangilaymiz
        employee.PasswordSalt = newSalt;
        employee.PasswordHash = newPasswordHash;

        _context.Users.Update(employee);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Password changed successfully");
    }


    public async Task<ApiResult<UserDto>> GetCurrentUserAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null || httpContext.User.Identity?.IsAuthenticated != true)
            return ApiResult<UserDto>.Failure(["User is not authenticated"]);

        // JWT token ichidagi userId claimni olamiz
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return ApiResult<UserDto>.Failure(["Invalid token"]);

        var userId = long.Parse(userIdClaim.Value);

        var employee = await _context.Users.FindAsync(userId);
        if (employee == null)
            return ApiResult<UserDto>.Failure(["User not found"]);

        var response = new UserDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            UserName = employee.UserName,
            PhoneNumber = employee.PhoneNumber,
            DateOfBirth = employee.DateOfBirth,
            UserRole = employee.UserRole,
            IsActive = employee.IsActive
        };

        return ApiResult<UserDto>.Success(response);
    }
}
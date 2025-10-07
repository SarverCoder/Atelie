using Atelie.Application.Helpers.PasswordHasher;
using Atelie.Application.Models;
using Atelie.Application.Models.Users;
using Atelie.Domain.Entities;
using Atelie.Domain.Exceptions;
using Atelie.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Atelie.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(DatabaseContext context, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResult<string>> CreateUserAsync(CreateUserDto dto)
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

    public async Task<UserDto> GetUserByIdAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _context.Users
                                            .AsNoTracking()
                                            .ToListAsync();

        return _mapper.Map<List<UserDto>>(users);

    }

    public async Task<ApiResult<string>> UpdateUserAsync(long id, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        _mapper.Map(dto, user);

        await _context.SaveChangesAsync();
        
        return ApiResult<string>.Success("User successfully updated");

    }

    public async Task<ApiResult<string>> DeleteUserAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("User successfully deleted");
    }
}
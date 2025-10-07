using Atelie.Application.Models.Users;
using Atelie.Domain.Entities;
using AutoMapper;

namespace Atelie.Application.MappingProfile;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();
    }
}
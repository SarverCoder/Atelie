using Atelie.Application.Models.Fabrics;
using Atelie.Domain.Entities;
using AutoMapper;

namespace Atelie.Application.MappingProfile;

public class FabricProfile : Profile
{
    public FabricProfile()
    {
        CreateMap<CreateFabricDto, Fabric>();
        CreateMap<UpdateFabricDto, Fabric>();
        CreateMap<Fabric, FabricDto>();
    }
}
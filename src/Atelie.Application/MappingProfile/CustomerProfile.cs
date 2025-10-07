using Atelie.Application.Models.Customers;
using Atelie.Domain.Entities;
using AutoMapper;

namespace Atelie.Application.MappingProfile;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<UpdateCustomerDto, Customer>();
    }
}
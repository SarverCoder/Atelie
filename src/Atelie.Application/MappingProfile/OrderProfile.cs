using Atelie.Application.Models.Orders;
using Atelie.Domain.Entities;
using AutoMapper;

namespace Atelie.Application.MappingProfile;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        // READ taraf (Order -> OrderDto)
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems))
            .ForMember(d => d.Workers, o => o.MapFrom(s => s.OrderWorkers));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.FabricName, o => o.MapFrom(s => s.Fabric != null ? s.Fabric.Name : null))
            .ForMember(dest => dest.OrderType, o => o.MapFrom(s => s.OrderType.ToString()));

        CreateMap<OrderWorker, OrderWorkerDto>()
            .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User.FullName))
            .ForMember(dest => dest.WorkerRole, o => o.MapFrom(s => s.WorkerRole.ToString()));


    }
}
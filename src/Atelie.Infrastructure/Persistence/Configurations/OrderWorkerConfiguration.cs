using Atelie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atelie.Infrastructure.Persistence.Configurations;

public class OrderWorkerConfiguration : IEntityTypeConfiguration<OrderWorker>
{
    public void Configure(EntityTypeBuilder<OrderWorker> builder)
    {
        builder
            .HasOne(ow => ow.Order)
            .WithMany(o => o.OrderWorkers)
            .HasForeignKey(ow => ow.OrderId);

        builder
            .HasOne(ow => ow.User)
            .WithMany(u => u.OrderWorkers)
            .HasForeignKey(ow => ow.UserId);
    }
}
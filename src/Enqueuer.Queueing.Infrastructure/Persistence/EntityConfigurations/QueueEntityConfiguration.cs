using Enqueuer.Queueing.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;

internal class QueueEntityConfiguration : IEntityTypeConfiguration<Queue>
{
    public void Configure(EntityTypeBuilder<Queue> builder)
    {
        builder.HasKey(q => q.Id);

        builder.HasMany(q => q.Participants)
            .WithOne()
            .HasForeignKey(p => p.QueueId);

        builder.ToTable("queues");
    }
}

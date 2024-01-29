using Enqueuer.Queueing.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;

internal class ParticipantEntityConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.OwnsOne(p => p.Position);

        builder.HasKey(p => new object[] { p.Id, p.Position.Number, p.Position.QueueId });

        builder.ToTable("participants");
    }
}

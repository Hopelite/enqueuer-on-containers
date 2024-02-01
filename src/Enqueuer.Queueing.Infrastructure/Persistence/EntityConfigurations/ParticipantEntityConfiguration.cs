using Enqueuer.Queueing.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;

internal class ParticipantEntityConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        //builder.OwnsOne(p => p.Position);

        builder.HasKey(p => new { p.Id, p.Number, p.QueueId });

        builder.HasOne(p => p.Queue)
            .WithMany(q => q.Participants);

        builder.ToTable("participants");
    }
}

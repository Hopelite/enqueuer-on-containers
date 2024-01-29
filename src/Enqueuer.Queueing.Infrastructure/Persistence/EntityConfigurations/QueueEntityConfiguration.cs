using Enqueuer.Queueing.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;

internal class QueueEntityConfiguration : IEntityTypeConfiguration<Queue>
{
    public void Configure(EntityTypeBuilder<Queue> builder)
    {
        builder.HasMany(q => q.Participants)
            .WithOne()
            .HasForeignKey(p => p.Position.QueueId);
    }
}

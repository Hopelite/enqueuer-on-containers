using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Queueing.Infrastructure.Persistence;

public class QueueingContext : DbContext
{
    public DbSet<Queue> Queues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new QueueEntityConfiguration())
            .ApplyConfiguration(new ParticipantEntityConfiguration());
    }
}

using Enqueuer.Queueing.Infrastructure.Persistence.Entities;
using Enqueuer.Queueing.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Queueing.Infrastructure.Persistence;

public class QueueingContext : DbContext
{
    public QueueingContext(DbContextOptions<QueueingContext> options)
        : base(options)
    {
    }

    public DbSet<Queue> Queues { get; set; }

    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new QueueEntityConfiguration())
            .ApplyConfiguration(new ParticipantEntityConfiguration());
    }
}

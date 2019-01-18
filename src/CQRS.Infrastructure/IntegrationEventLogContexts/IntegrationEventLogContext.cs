using CQRS.Infrastructure.Idempotency;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Api.Infrastructure.IntegrationEventLogContexts
{
    public class IntegrationEventLogContext : DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
        }

        public DbSet<ClientRequest> ClientRequests { get; set; }
    }
}

using CQRS.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Infrastructure.IntegrationEventLogContexts
{
    public class IntegrationEventLogContext : DbContext
    {
        public DbSet<ClientRequest> ClientRequests { get; set; }
    }
}

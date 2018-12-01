using System;

namespace CQRS.Infrastructure.Idempotency
{
    public class ClientRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}

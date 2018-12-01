using CQRS.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly CQRSDomainContext _context;

        public RequestManager(CQRSDomainContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);
            var request = exists ?
                throw new CQRSDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTimeOffset .UtcNow
                };
            _context.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);
            return request != null;
        }
    }
}

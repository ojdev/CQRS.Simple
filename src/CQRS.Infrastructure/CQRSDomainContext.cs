using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Infrastructure
{
    public class CQRSDomainContext : DbContext
    {
        private readonly IMediator _mediator;
        private CQRSDomainContext(DbContextOptions<CQRSDomainContext> options) : base(options) { }
        public CQRSDomainContext(DbContextOptions<CQRSDomainContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            System.Diagnostics.Debug.WriteLine("CQRSDomainContext::ctor ->" + this.GetHashCode());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries().Where(t => t.State == EntityState.Deleted || t.State == EntityState.Modified))
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        {
                            entry.CurrentValues["DeletionTime"] = DateTimeOffset.Now;
                            entry.CurrentValues["IsDeleted"] = true;
                            entry.State = EntityState.Modified;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entry.CurrentValues["LastUpdateTime"] = DateTimeOffset.Now;
                            break;
                        }
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            bool saveResult = false;
            try
            {
                // Dispatch Domain Events collection. 
                // Choices:
                // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
                // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
                // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
                // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
                await _mediator.DispatchDomainEventsAsync(this);

                // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
                // performed throught the DbContext will be commited
                var result = await base.SaveChangesAsync();
                saveResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (saveResult)
                {
                    await _mediator.DispatchDomainEventsAsync(this);
                }
            }
            return saveResult;
        }
    }
}

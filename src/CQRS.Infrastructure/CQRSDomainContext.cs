using EFCore.Kit.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Infrastructure
{
    public class CQRSDomainContext : RDbContext
    {
        //private readonly IMediator _mediator;

        public CQRSDomainContext(DbContextOptions<CQRSDomainContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

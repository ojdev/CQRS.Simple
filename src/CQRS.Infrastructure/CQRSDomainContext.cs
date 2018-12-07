using EFCore.Kit.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Infrastructure
{
    public class CQRSDomainContext : KitDbContext
    {
        private readonly IMediator _mediator;

        public CQRSDomainContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

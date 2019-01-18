using CQRS.Infrastructure.Idempotency;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Api.Application.Commands
{
    public class CreateDomainCommandHandler : IRequestHandler<CreateDomainCommand, bool>
    {
        private readonly IMediator _mediator;

        public CreateDomainCommandHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<bool> Handle(CreateDomainCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("CreateDomainCommand"); ;
            return Task.FromResult(true);
        }
    }

    public class CreateDomainIdentifiedCommandHandler : IdentifiedCommandHandler<CreateDomainCommand, bool>
    {
        public CreateDomainIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}

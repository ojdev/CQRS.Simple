using MediatR;
using System;

namespace CQRS.Api.Application.Commands
{
    public class IdentifiedCommand<TCommand, RCommand> : IRequest<RCommand> where TCommand : IRequest<RCommand>
    {
        public TCommand Command { get; }
        public Guid Id { get; }

        public IdentifiedCommand(TCommand command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}

using CQRS.Infrastructure.Idempotency;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Api.Application.Commands
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="R">Return value of the inner command handler</typeparam>
    public class IdentifiedCommandHandler<TCommand, RCommand> : IRequestHandler<IdentifiedCommand<TCommand, RCommand>, RCommand> where TCommand : IRequest<RCommand>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual RCommand CreateResultForDuplicateRequest()
        {
            return default(RCommand);
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<RCommand> Handle(IdentifiedCommand<TCommand, RCommand> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<TCommand>(message.Id);
                try
                {
                    // Send the embeded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(message.Command);
                    return result;
                }
                catch
                {
                    return default(RCommand);
                }
            }
        }
    }
}

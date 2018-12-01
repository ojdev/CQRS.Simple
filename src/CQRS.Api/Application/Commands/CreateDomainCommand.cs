using MediatR;
using System.Runtime.Serialization;

namespace CQRS.Api.Application.Commands
{
    [DataContract]
    public class CreateDomainCommand : IRequest<bool>
    {

    }
}

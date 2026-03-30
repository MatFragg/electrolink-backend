using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CannotCancelAssignedRequestException : DomainException
{
    public CannotCancelAssignedRequestException(string requestId)
        : base($"Cannot cancel request {requestId} because it is already assigned.") { }

    public CannotCancelAssignedRequestException(RequestId requestId)
        : base($"Cannot cancel request {requestId.Value} because it is already assigned.") { }

    public CannotCancelAssignedRequestException(string message, Exception inner)
        : base(message, inner) { }
}

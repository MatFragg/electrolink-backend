namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidCancellationReasonException : DomainException
{
    public InvalidCancellationReasonException(string raw)
        : base($"Invalid cancellation reason: {raw}") { }
}
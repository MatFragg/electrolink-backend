namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidDeactivationReasonException : DomainException
{
    public InvalidDeactivationReasonException(string raw)
        : base($"Invalid deactivation reason: {raw}") { }
}
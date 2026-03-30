namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidDurationException : DomainException
{
    public InvalidDurationException(string message)
        : base(message) { }

    public InvalidDurationException(int minutes)
        : base($"Invalid duration: {minutes} minutes") { }
}
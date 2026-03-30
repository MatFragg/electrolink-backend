namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidPreferredDateException : DomainException
{
    public InvalidPreferredDateException()
        : base("One or more preferred dates are invalid or too soon.") { }

    public InvalidPreferredDateException(string message)
        : base(message) { }
}
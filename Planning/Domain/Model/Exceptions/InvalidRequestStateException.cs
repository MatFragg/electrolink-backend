namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidRequestStateException : InvalidOperationException
{
    public InvalidRequestStateException(string message) : base(message) { }
}


using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Exceptions;

[Serializable]
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() { }

    public InvalidCredentialsException(string? message) : base(message) { }

    public InvalidCredentialsException(string? message, Exception? innerException) : base(message, innerException) { }

    protected InvalidCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

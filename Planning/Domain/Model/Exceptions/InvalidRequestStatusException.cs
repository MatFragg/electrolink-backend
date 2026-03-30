namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidRequestStatusException : DomainException
{
    public object RequestId { get; }
    public object Expected { get; }
    public object Actual { get; }

    public InvalidRequestStatusException(object requestId, object expected, object actual)
        : base($"Invalid request status for {requestId}. Expected: {expected}, Actual: {actual}")
    {
        RequestId = requestId;
        Expected = expected;
        Actual = actual;
    }

    public InvalidRequestStatusException(string message) : base(message) { }
}
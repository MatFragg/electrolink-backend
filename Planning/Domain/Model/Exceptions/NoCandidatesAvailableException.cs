namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class NoCandidatesAvailableException : Exception
{
    public NoCandidatesAvailableException(string message) : base(message)
    {
    }
}
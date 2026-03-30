namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class ServiceRequestNotFoundException : Exception
{
    public ServiceRequestNotFoundException(string message) : base(message)
    {
    }
}

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidPricingException : Exception
{
    public InvalidPricingException(string message) : base(message)
    {
    }
}
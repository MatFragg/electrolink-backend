namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidWarrantyPeriodException : DomainException
{
    public InvalidWarrantyPeriodException(string message)
        : base(message) { }

    public InvalidWarrantyPeriodException(int months)
        : base($"Invalid warranty period: {months} months") { }
}
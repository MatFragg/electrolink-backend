namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidQuantityException : DomainException
{
    public InvalidQuantityException(string componentTypeId)
        : base($"Invalid quantity for component type {componentTypeId}") { }

    public InvalidQuantityException(int quantity)
        : base($"Invalid quantity: {quantity}") { }
}
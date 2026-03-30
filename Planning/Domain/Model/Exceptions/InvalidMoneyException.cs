namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidMoneyException : DomainException
{
    public InvalidMoneyException(string message)
        : base(message) { }

    public InvalidMoneyException(decimal amount)
        : base($"Invalid money amount: {amount}") { }
}
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CurrencyMismatchException : DomainException
{
    public ECurrency Expected { get; }
    public ECurrency Actual { get; }

    public CurrencyMismatchException(ECurrency expected, ECurrency actual)
        : base($"Currency mismatch: expected {expected}, but got {actual}")
    {
        Expected = expected;
        Actual = actual;
    }
}
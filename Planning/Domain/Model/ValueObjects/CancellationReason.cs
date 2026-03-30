using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record CancellationReason
{
    public ECancellationReason Value { get; }
    private CancellationReason(ECancellationReason value) => Value = value;

    public static CancellationReason From(string raw)
    {
        if (!Enum.TryParse<ECancellationReason>(raw, ignoreCase: true, out var parsed))
            throw new InvalidCancellationReasonException(raw);
        return new CancellationReason(parsed);
    }
}



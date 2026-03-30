using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record DeactivationReason
{
    public EDeactivationReason Value { get; }
    private DeactivationReason(EDeactivationReason value) => Value = value;

    public static DeactivationReason From(string raw)
    {
        if (!Enum.TryParse<EDeactivationReason>(raw, ignoreCase: true, out var parsed))
            throw new InvalidDeactivationReasonException(raw);
        return new DeactivationReason(parsed);
    }
}


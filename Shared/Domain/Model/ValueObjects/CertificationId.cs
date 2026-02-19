using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record CertificationId
{
    public string Value { get; init; }

    private CertificationId(string value) => Value = value;

    public static CertificationId NewCertificationId() => new($"cert-{Guid.NewGuid()}");

    public static CertificationId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("cert-"))
            throw new InvalidIdException("CertificationId", value);
        return new CertificationId(value);
    }

    public override string ToString() => Value;
}
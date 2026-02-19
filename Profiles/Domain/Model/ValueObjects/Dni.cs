using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record Dni
{
    public string Value { get; init; }

    private Dni(string value) => Value = value;

    public static Dni From(string raw)
    {
        var cleaned = raw.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^\d{8}$"))
            throw new InvalidDniException(raw);
        return new Dni(cleaned);
    }

    public override string ToString() => Value;
}
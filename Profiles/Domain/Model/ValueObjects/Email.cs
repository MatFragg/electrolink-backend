using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record Email
{
    public string Value { get; init; }

    private Email(string value) => Value = value;

    public static Email From(string raw)
    {
        if (!IsValidEmail(raw))
            throw new InvalidEmailException(raw);
        return new Email(raw.ToLowerInvariant().Trim());
    }

    private static bool IsValidEmail(string email)
        => System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");

    public override string ToString() => Value;
}
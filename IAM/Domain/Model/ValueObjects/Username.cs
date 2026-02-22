using System.Text.RegularExpressions;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;

public record Email
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private Email(string value) => Value = value;

    public static Email Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Email cannot be empty.");

        if (!EmailRegex.IsMatch(raw))
            throw new ArgumentException("Email must be a valid email address.");

        return new Email(raw.ToLowerInvariant().Trim());
    }
}
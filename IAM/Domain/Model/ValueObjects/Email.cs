using System.Text.RegularExpressions;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;

public record Email
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private Email(string value) => Value = value;

    public static Email From(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Email cannot be empty.");

        if (!IsValidEmail(raw))
            throw new InvalidEmailException(raw);

        return new Email(raw.ToLowerInvariant().Trim());
    }
    
    private static bool IsValidEmail(string email)
        => EmailRegex.IsMatch(email);
}
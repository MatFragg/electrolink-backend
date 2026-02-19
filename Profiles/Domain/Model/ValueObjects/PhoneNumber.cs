using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record PhoneNumber
{
    public string Value { get; init; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber From(string raw)
    {
        // E.164: +[country code][number]
        if (!System.Text.RegularExpressions.Regex.IsMatch(raw, @"^\+[1-9]\d{7,14}$"))
            throw new InvalidPhoneNumberException(raw);
        return new PhoneNumber(raw);
    }

    public override string ToString() => Value;
}
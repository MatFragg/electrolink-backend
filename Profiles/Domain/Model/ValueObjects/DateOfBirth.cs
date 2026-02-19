using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record DateOfBirth
{
    public DateOnly Value { get; init; }

    private DateOfBirth(DateOnly value) => Value = value;

    public static DateOfBirth From(string raw)
    {
        if (!DateOnly.TryParse(raw, out var date))
            throw new InvalidDateOfBirthException(raw);

        var age = CalculateAge(date);
        if (age < 18) throw new UnderageUserException(age);

        return new DateOfBirth(date);
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age   = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age;
    }
}
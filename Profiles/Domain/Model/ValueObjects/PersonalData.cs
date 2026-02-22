namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record PersonalData
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public PhoneNumber PhoneNumber { get; init; }
    public Dni Dni { get; init; }
    public DateOfBirth DateOfBirth { get; init; }
    public Address Address { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    private PersonalData() { }

    public static PersonalData Create(string firstName, string lastName, PhoneNumber phoneNumber, Dni dni, DateOfBirth dateOfBirth, Address address) =>
        new()
        {
            FirstName   = firstName.Trim(),
            LastName    = lastName.Trim(),
            PhoneNumber = phoneNumber,
            Dni         = dni,
            DateOfBirth = dateOfBirth,
            Address     = address
        };

    public PersonalData Update(string? firstName = null, string? lastName = null, PhoneNumber? phoneNumber = null, Address? address = null) =>
        this with
        {
            FirstName   = firstName?.Trim() ?? FirstName,
            LastName    = lastName?.Trim()  ?? LastName,
            PhoneNumber = phoneNumber         ?? PhoneNumber,
            Address     = address             ?? Address,
        };
}
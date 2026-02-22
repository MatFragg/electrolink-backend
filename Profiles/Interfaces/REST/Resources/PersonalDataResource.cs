namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record PersonalDataResource(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni,
    string DateOfBirth,
    string Street,
    string District,
    string City,
    string Country,
    string PostalCode
    );
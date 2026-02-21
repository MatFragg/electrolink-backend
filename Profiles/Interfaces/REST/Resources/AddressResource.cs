namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record AddressResource(
    string Street,
    string District,
    string City,
    string Country,
    string PostalCode
    );
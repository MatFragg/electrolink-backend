namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record AddressResource(
    string Street, 
    string Number,
    string District, 
    string City, 
    string Country, 
    string PostalCode
);
namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record AddressResource(
    string Street, 
    string Number, 
    string City, 
    string PostalCode, 
    string Country, 
    decimal Latitude, 
    decimal Longitude
);
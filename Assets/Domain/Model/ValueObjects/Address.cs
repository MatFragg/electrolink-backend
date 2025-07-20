using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

[Owned]
public record Address(
    string Street, 
    string Number, 
    string City, 
    string PostalCode, 
    string Country, 
    decimal Latitude, 
    decimal Longitude
);
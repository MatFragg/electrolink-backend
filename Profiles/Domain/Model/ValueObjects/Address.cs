namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record Address
{
    public string Street { get; init; }
    public string District { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public string PostalCode { get; init; }

    private Address()
    {
    }

    public static Address Create(string street, string district, string city, string country, string postalCode) =>
        new()
        {
            Street = street.Trim(),
            District = district.Trim(),
            City = city.Trim(),
            Country = country.Trim(),
            PostalCode = postalCode.Trim()
        };

    public Address Update(string? street = null, string? district = null, string? city = null, string? country = null,
        string? postalCode = null) =>
        this with
        {
            Street = street?.Trim() ?? Street,
            District = district?.Trim() ?? District,
            City = city?.Trim() ?? City,
            Country = country?.Trim() ?? Country,
            PostalCode = postalCode?.Trim() ?? PostalCode
        };
}
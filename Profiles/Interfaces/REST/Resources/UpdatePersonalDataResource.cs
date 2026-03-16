namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de request para PATCH /me/personal-data.
/// Todos los campos son opcionales (actualización parcial).
/// Nota: Dni y DateOfBirth NO son actualizables.
/// </summary>
public record UpdatePersonalDataResource(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Street,
    string? Number,
    string? District,
    string? City,
    string? Country,
    string? PostalCode);


namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de request para DELETE /me (desactivación de perfil).
/// </summary>
public record DeactivateProfileResource(
    string Reason,
    string? Notes = null);


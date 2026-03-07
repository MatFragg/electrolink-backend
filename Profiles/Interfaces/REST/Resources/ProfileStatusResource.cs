namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de respuesta para GET /me/status.
/// Envuelve el ProfileStatusReadModel para la capa REST.
/// </summary>
public record ProfileStatusResource(
    string ProfileId,
    string UserId,
    string Status,
    int CompletionPercentage);


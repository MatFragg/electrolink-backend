namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record DeactivateProfileCommand(
    string ProfileId,
    string UserId,
    string Reason,
    string? Notes = null);
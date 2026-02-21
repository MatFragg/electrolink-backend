namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record ReactivateProfileCommand(
    string ProfileId,
    string UserId);
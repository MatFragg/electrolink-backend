namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;

public sealed record ProfileStatusReadModel(
    string ProfileId,
    string UserId,
    string Status,
    int CompletionPercentage
);
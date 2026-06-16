namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ReadModels;

public sealed record ProfileStatusReadModel(
    string ProfileId,
    string UserId,
    string Status,
    int CompletionPercentage
);
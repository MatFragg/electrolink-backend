using Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class ProfileStatusResourceFromReadModelAssembler
{
    public static ProfileStatusResource ToResourceFromReadModel(
        ProfileStatusReadModel readModel) =>
        new(
            ProfileId:            readModel.ProfileId,
            UserId:               readModel.UserId,
            Status:               readModel.Status,
            CompletionPercentage: readModel.CompletionPercentage);
}


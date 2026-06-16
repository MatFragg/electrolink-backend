using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record SetPropertyMainPhotoCommand(
    HomeownerId HomeownerId,
    PropertyId PropertyId,
    string ProviderId);

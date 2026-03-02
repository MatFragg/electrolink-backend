using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddPhotoToPropertyCommand(PropertyId PropertyId, string? PhotoUrl);

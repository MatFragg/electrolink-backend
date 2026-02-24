namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddPhotoToPropertyCommand(string PropertyId, string? PhotoUrl);

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddPhotoToPropertyCommand(Guid Id, string? PhotoUrl);

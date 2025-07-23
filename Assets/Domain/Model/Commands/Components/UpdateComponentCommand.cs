namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;

public record UpdateComponentCommand(Guid Id, string Name, string? Description, int TypeId, bool IsActive);

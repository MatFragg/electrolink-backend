namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;

public record UpdateComponentCommand(string ComponentId, string Name, string? Description, string ComponentTypeId, bool IsActive);

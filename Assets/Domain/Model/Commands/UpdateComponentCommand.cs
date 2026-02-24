namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentCommand(string ComponentId, string Name, string? Description, string ComponentTypeId, bool IsActive);

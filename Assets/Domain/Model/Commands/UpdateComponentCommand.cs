namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentCommand(Guid Id, string Name, string? Description, int TypeId, bool IsActive);

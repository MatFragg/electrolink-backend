namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record UpdateComponentResource(string Name, string? Description, int TypeId, bool IsActive);

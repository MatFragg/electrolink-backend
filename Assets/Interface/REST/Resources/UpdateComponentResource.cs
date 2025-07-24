namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record UpdateComponentResource(string Name, string? Description, bool IsActive, int TypeId);

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record CreateComponentResource(string Name, string Description,bool IsActive, string ComponentTypeId);

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record CreateComponentResource(string Name, string Description,bool IsActive, int ComponentTypeId);

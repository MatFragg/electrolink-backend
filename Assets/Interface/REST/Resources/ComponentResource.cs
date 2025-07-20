namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record ComponentResource(Guid Id, string Name, string Description, bool IsActive, int TypeId);

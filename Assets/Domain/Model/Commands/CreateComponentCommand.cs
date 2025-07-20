namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record CreateComponentCommand(string Name, string Description, int ComponentTypeId);

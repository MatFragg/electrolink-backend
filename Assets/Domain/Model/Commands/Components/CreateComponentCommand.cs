namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;

public record CreateComponentCommand(string Name, string Description,bool IsActive, int ComponentTypeId);

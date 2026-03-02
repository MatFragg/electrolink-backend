using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record CreateComponentCommand(string Name, string Description,bool IsActive, ComponentTypeId ComponentTypeId);

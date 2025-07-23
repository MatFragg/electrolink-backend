using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IComponentCommandService
{
    Task<Component?> Handle(CreateComponentCommand command);
    Task<Component?> Handle(UpdateComponentCommand command);
    Task<bool> Handle(DeleteComponentCommand command);
}
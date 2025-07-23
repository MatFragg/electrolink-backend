using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IComponentTypeCommandService
{
    Task<ComponentType?> Handle(CreateComponentTypeCommand command);
    Task<ComponentType?> Handle(UpdateComponentTypeCommand command);
    Task<bool> Handle(DeleteComponentTypeCommand command);
}
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class ComponentCommandService(IComponentRepository componentRepository, IComponentTypeRepository componentTypeRepository, IUnitOfWork unitOfWork) : IComponentCommandService
{
    public async Task<Component?> Handle(CreateComponentCommand command)
    {
        
        var componentType = await componentTypeRepository.FindByIdAsync(new ComponentTypeId(command.ComponentTypeId));
        if (componentType is null)
            throw new ArgumentException($"Component type with id {command.ComponentTypeId} not found.");

        // 2. ¡NUEVA VALIDACIÓN! Validar que no exista ya un componente con el mismo nombre.
        if (await componentRepository.ExistsByNameAsync(command.Name))
            throw new ArgumentException($"A component with the name '{command.Name}' already exists.");

        var component = new Component(command);
        await componentRepository.AddAsync(component);
        await unitOfWork.CompleteAsync();
        return component;
    }

    public async Task<Component?> Handle(UpdateComponentCommand command)
    {
        var component = await componentRepository.FindByIdAsync(new (command.Id));
        if (component is null) throw new ArgumentException("Component not found.");

        component.UpdateInfo(command);
        await unitOfWork.CompleteAsync();
        return component;
    }

    public async Task<bool> Handle(DeleteComponentCommand command)
    {
        var component = await componentRepository.FindByIdAsync(new ComponentId(command.Id));
        if (component is null)
        {
            return false;
        }
        
        component.Deactivate();
        await unitOfWork.CompleteAsync();

        return true;
    }
}
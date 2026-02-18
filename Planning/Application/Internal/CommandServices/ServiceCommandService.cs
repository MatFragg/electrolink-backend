using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceCommandService(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator
) : IServiceCommandService
{
    public async Task<Service> Handle(CreateServiceCommand command)
    {
        // Create Value Objects
        var basePrice = new Money(command.BasePrice, "USD");
        var createdBy = new TechnicianId(command.CreatedBy);

        // Use Factory Method
        var service = Service.Create(
            command.Name,
            command.Description,
            basePrice,
            command.EstimatedTime,
            command.Category,
            createdBy,
            new ServicePolicy("default", "default"));  // Crear ServicePolicy predeterminado en lugar de usar bool

        // Establecer visibilidad después de la creación
        if (command.IsVisible)
            service.Show();
        else
            service.Hide();

        await serviceRepository.AddAsync(service);
        await unitOfWork.CompleteAsync();

        // Publish Domain Events
        foreach (var domainEvent in service.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        service.ClearDomainEvents();

        return service;
    }

    public async Task<Service?> Handle(UpdateServiceCommand command)
    {
        var serviceId = new ServiceId(command.ServiceId);
        var service = await serviceRepository.FindByIdAsync(serviceId);
        if (service == null) return null;

        var newBasePrice = new Money(command.BasePrice, "USD");

        // Update uses the full signature including policy, restriction, tags, and components
        // We preserve existing collections if not provided in command
        service.Update(
            command.Name,
            command.Description,
            newBasePrice,
            command.EstimatedTime,
            command.Category,
            service.Policy ?? new ServicePolicy("default", "default"),  // Usar string en lugar de int
            service.Restriction ?? new ServiceRestriction(new List<string>(), new List<string>(), false),  // Usar listas vacías y bool para el tercer parámetro
            service.Tags,
            service.Components);

        // Update visibility separately
        if (command.IsVisible)
            service.Show();
        else
            service.Hide();

        serviceRepository.Update(service);
        await unitOfWork.CompleteAsync();

        // Publish Domain Events
        foreach (var domainEvent in service.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        service.ClearDomainEvents();

        return service;
    }

    public async Task<bool> Handle(DeleteServiceCommand command)
    {
        var serviceId = new ServiceId(command.ServiceId);
        var service = await serviceRepository.FindByIdAsync(serviceId);
        if (service == null) return false;

        serviceRepository.Remove(service);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
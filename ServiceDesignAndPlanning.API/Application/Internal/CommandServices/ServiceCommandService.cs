using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.CommandServices;

public class ServiceCommandService(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork
) : IServiceCommandService
{
    public async Task<Service> Handle(CreateServiceCommand command)
    {
        var service = new Service(command);
        await serviceRepository.AddAsync(service);
        await unitOfWork.CompleteAsync();
        return service;
    }
    public async Task<Service?> UpdateAsync(UpdateServiceCommand command)
    {
        var service = await serviceRepository.FindByIdAsync(command.ServiceId);
        if (service == null) return null;

        service.UpdateDetails(command.Name, command.Description, command.BasePrice,
            command.EstimatedTime, command.Category, command.IsVisible);

        serviceRepository.Update(service);
        await unitOfWork.CompleteAsync();
        return service;
    }

    public async Task<bool> DeleteAsync(DeleteServiceCommand command)
    {
        var service = await serviceRepository.FindByIdAsync(command.ServiceId);
        if (service == null) return false;

        serviceRepository.Remove(service);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
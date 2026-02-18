using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class RequestCommandService(
    IRequestRepository requestRepository,
    ISubscriptionsContextFacade subscriptionsFacade,
    IUnitOfWork unitOfWork,
    IMediator mediator
) : IRequestCommandService
{
    public async Task<Request> Handle(CreateRequestCommand command)
    {
        // Create domain value objects
        var clientId = new ClientId(command.ClientId);
        var serviceId = new ServiceId(command.ServiceId);
        var propertyId = new PropertyId(command.PropertyId);

        // Get current month usage and plan info via ACL
        var now = DateTime.UtcNow;
        var currentMonthUsage = await requestRepository.CountByClientIdAndMonthAsync(
            clientId, 
            now.Year, 
            now.Month);

        var planInfo = await subscriptionsFacade.GetSubscriptionPlanByClientIdAsync(clientId);
        bool isPremiumUser = planInfo?.IsBasicPlan == false;

        // Use Factory Method to create Request (includes validation)
        var request = Request.Create(
            clientId,
            propertyId,
            serviceId,
            command.ScheduledDate,
            command.ProblemDescription,
            isPremiumUser,
            currentMonthUsage,
            null); // bill is optional

        // Persist
        await requestRepository.AddAsync(request);
        await unitOfWork.CompleteAsync();

        // Publish Domain Events
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();

        return request;
    }

    public async Task<Request?> UpdateAsync(UpdateRequestCommand command)
    {
        var requestId = new RequestId(command.RequestId);
        var request = await requestRepository.FindByIdAsync(requestId);
        if (request is null) return null;

        // Use aggregate methods
        if (command.ScheduledDate.HasValue)
        {
            request.UpdateScheduledDate(command.ScheduledDate.Value);
        }

        if (command.TechnicianId.HasValue)
        {
            var technicianId = new TechnicianId(command.TechnicianId.Value);
            request.AssignTechnician(technicianId);
        }

        if (!string.IsNullOrEmpty(command.ProblemDescription))
        {
            request.UpdateProblemDescription(command.ProblemDescription);
        }

        await requestRepository.UpdateAsync(request);
        await unitOfWork.CompleteAsync();

        // Publish Domain Events
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();

        return request;
    }

    public async Task<bool> DeleteAsync(DeleteRequestCommand command)
    {
        var requestId = new RequestId(command.RequestId);
        var request = await requestRepository.FindByIdAsync(requestId);
        if (request is null) return false;

        await requestRepository.DeleteAsync(request);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
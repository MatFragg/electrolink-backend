using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal;

public class SubscriptionCommandService(
    ISubscriptionRepository _subscriptionRepository,
    IUnitOfWork unitOfWork,
    IProfilesContextFacade profilesContextFacade
) : ISubscriptionCommandService
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command)
    {
        // Obtener TechnicianId y ProfileId desde el ACL usando el ProfileId
        var technicianInfo = await profilesContextFacade.GetTechnicianInfoByProfileIdAsync(command.UserId);
        if (technicianInfo is null)
            throw new InvalidOperationException($"Technician not found for profile {command.UserId}");

        // Desestructuramos la tupla
        var (technicianId, profileId) = technicianInfo.Value;

        // Crear la suscripción
        var subscription = new Subscription(
            new UserId(profileId), // 👈 este es el int (UserId)
            new PlanId(command.PlanId)
        );

        await _subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();

        return subscription.Id.Value;
    }


    public async Task Handle(CancelSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        subscription?.Cancel();
        _subscriptionRepository.Update(subscription!);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(PauseSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        subscription?.Pause();
        _subscriptionRepository.Update(subscription!);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(ResumeSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        subscription?.Resume();
        _subscriptionRepository.Update(subscription!);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(GrantPremiumAccessCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        subscription?.GrantPremiumAccess(command.ValidUntil);
        _subscriptionRepository.Update(subscription!);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(ActivateBoostCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        if (subscription != null && subscription.CanActivateBoost(DateTime.UtcNow))
        {
            subscription.ActivateBoost(DateTime.UtcNow);
            _subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync();
        }
    }

    public async Task Handle(VerifyCertificationCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
        subscription?.VerifyCertification(DateTime.UtcNow);
        _subscriptionRepository.Update(subscription!);
        await unitOfWork.CompleteAsync();
    }
}
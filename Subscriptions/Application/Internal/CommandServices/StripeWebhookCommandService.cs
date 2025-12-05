using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;
using Stripe;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

/// <summary>
/// Service to manage commands related to Stripe webhooks.
/// </summary>
public class StripeWebhookCommandService(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository, IUnitOfWork unitOfWork, ILogger<StripeWebhookCommandService> logger) : IStripeWebhookCommandService
{ 
    /// <summary>
    /// Sync a subscription from Stripe to our DB.
    /// </summary>
    public async Task Handle(SyncSubscriptionFromStripeCommand command)
    {
        logger.LogInformation(
            "Syncing subscription from Stripe: {StripeSubscriptionId}",
            command.StripeSubscriptionId);

        // 1. Search for existing subscription by StripeSubscriptionId
        var subscription = await subscriptionRepository
            .FindByStripeSubscriptionIdAsync(command.StripeSubscriptionId);

        if (subscription == null)
        {
            logger.LogWarning(
                "Subscription not found in DB for Stripe ID {StripeSubscriptionId}. " +
                "This can be normal if it was created directly in Stripe.",
                command.StripeSubscriptionId);

            // TODO: Optionally, create the subscription if it doesn't exist
            // This would require looking up the Customer by StripeCustomerId and associating it with a UserId
            return;
        }

        // 2. Map Stripe status to our domain status
        var newStatus = MapStripeStatusToSubscriptionStatus(command.Status);

        // 3. Update the subscription
        subscription.UpdateStatus(newStatus);
        subscription.UpdateEndDate(command.CurrentPeriodEnd);
        subscription.UpdateTrialEndsAt(command.TrialEnd);

        // 4. Handle cancellation
        if (command.CancelAtPeriodEnd && command.CancelAt.HasValue)
        {
            subscription.ScheduleCancellation(command.CancelAt.Value);
        }

        // 5. Persist changes
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        logger.LogInformation(
            "Subscription {SubscriptionId} synced: Status {Status}, EndDate {EndDate}",
            subscription.Id,
            newStatus,
            command.CurrentPeriodEnd);

        // 6. Domain events will be automatically published in UnitOfWork.CompleteAsync()
        // If we change the status, SubscriptionStatusChangedEvent will be published
    }

    private ESubscriptionStatus MapStripeStatusToSubscriptionStatus(string stripeStatus)
    {
        return stripeStatus.ToLowerInvariant() switch
        {
            "active" => ESubscriptionStatus.Active,
            "trialing" => ESubscriptionStatus.Trial,
            "past_due" => ESubscriptionStatus.PaymentDue,
            "canceled" => ESubscriptionStatus.Cancelled,
            "unpaid" => ESubscriptionStatus.Expired,
            "incomplete" => ESubscriptionStatus.Pending,
            "incomplete_expired" => ESubscriptionStatus.Expired,
            _ => ESubscriptionStatus.Pending
        };
    }
}
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.BackgroundServices;

public class GracePeriodExpirationJob(
    IServiceScopeFactory scopeFactory,
    ILogger<GracePeriodExpirationJob> logger) : BackgroundService
{
    private const int BatchSize = 100;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
                await ProcessExpiredSubscriptionsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task ProcessExpiredSubscriptionsAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var expired = await repository.FindAllInGracePeriodExpiredAsync(
            DateTime.UtcNow, BatchSize, 0);

        if (!expired.Any())
        {
            logger.LogInformation("No expired grace-period subscriptions found.");
            return;
        }

        logger.LogInformation("Found {Count} expired grace-period subscriptions. Processing...", expired.Count());

        foreach (var subscription in expired)
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                subscription.Degrade(
                    DegradationReason.From("PAYMENT_FAILURE"),
                    DateTime.UtcNow);
                repository.Update(subscription);
                await unitOfWork.CompleteAsync();

                foreach (var domainEvent in subscription.DomainEvents)
                    await mediator.Publish(domainEvent, stoppingToken);
                subscription.ClearDomainEvents();
            }
            catch (DbUpdateConcurrencyException)
            {
                logger.LogWarning("Concurrency conflict for subscription {Id}, already processed by another instance.", subscription.SubscriptionId.Value);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                logger.LogError(ex, "Error degrading subscription {Id}", subscription.SubscriptionId.Value);
            }
        }

        logger.LogInformation("Grace period expiration batch completed.");
    }
}

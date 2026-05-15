using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.BackgroundServices;

public class GracePeriodExpirationJob(
    IServiceScopeFactory scopeFactory,
    ILogger<GracePeriodExpirationJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);

            using var scope = scopeFactory.CreateScope();
            var commandService = scope.ServiceProvider.GetRequiredService<ISubscriptionCommandService>();
            var repository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();

            var expired = await repository.FindAllInGracePeriodExpiredAsync(DateTime.UtcNow);
            foreach (var subscription in expired)
            {
                try
                {
                    await commandService.Handle(new DegradeSubscriptionCommand(
                        StripeSubscriptionId: subscription.StripeSubscriptionId!.Value,
                        Reason: "PAYMENT_FAILURE",
                        DegradedAt: DateTime.UtcNow));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error degrading subscription {SubscriptionId}", subscription.SubscriptionId.Value);
                }
            }
        }
    }
}


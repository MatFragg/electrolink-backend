using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.BackgroundServices;

public class MonthlyCounterResetJob(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1);
            var delay = nextRun - now;

            await Task.Delay(delay, stoppingToken);

            using var scope = scopeFactory.CreateScope();
            var commandService = scope.ServiceProvider.GetRequiredService<ISubscriptionCommandService>();

            await commandService.Handle(new ResetMonthlyRequestCountersCommand());
        }
    }
}


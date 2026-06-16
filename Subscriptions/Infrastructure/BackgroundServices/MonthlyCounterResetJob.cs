using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.BackgroundServices;

public class MonthlyCounterResetJob(IServiceScopeFactory scopeFactory, ILogger<MonthlyCounterResetJob> logger) : BackgroundService
{
    private DateTime _lastRunUtc = DateTime.MinValue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CatchUpIfNeededAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1);
            var delay = nextRun - now;

            logger.LogInformation("Monthly counter reset job scheduled. Next run: {NextRun:O} (in {Delay:g})", nextRun, delay);

            await Task.Delay(delay, stoppingToken);
            await ResetCountersAsync(stoppingToken);
        }
    }

    private async Task CatchUpIfNeededAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var firstOfCurrentMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        if (_lastRunUtc < firstOfCurrentMonth)
        {
            logger.LogInformation("Monthly counter reset job detected a missed execution for month {Month}. Running catch-up.", firstOfCurrentMonth.ToString("yyyy-MM"));
            await ResetCountersAsync(stoppingToken);
        }
    }

    private async Task ResetCountersAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var commandService = scope.ServiceProvider.GetRequiredService<ISubscriptionCommandService>();

        await commandService.Handle(new ResetMonthlyRequestCountersCommand(DateTime.UtcNow));
        _lastRunUtc = DateTime.UtcNow;

        logger.LogInformation("Monthly request counters reset completed at {Now:O}", _lastRunUtc);
    }
}


using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class WebhookEventRepository(AppDbContext context) : IWebhookEventRepository
{

    public async Task<WebhookEvent?> FindByIdAsync(WebhookEventId id)
    {
        return await context.Set<WebhookEvent>()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task AddAsync(WebhookEvent webhookEvent)
    {
        await context.Set<WebhookEvent>().AddAsync(webhookEvent);
    }

    public void Update(WebhookEvent webhookEvent)
    {
        context.Set<WebhookEvent>().Update(webhookEvent);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}
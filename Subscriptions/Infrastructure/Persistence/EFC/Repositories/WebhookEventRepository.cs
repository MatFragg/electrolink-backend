using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class WebhookEventRepository(AppDbContext context) : BaseRepository<WebhookEvent, WebhookEventId>(context), IWebhookEventRepository
{

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}
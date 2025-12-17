using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

/// <summary>
/// Repository for WebhookEvent (idempotency de webhooks).
/// </summary>
public interface IWebhookEventRepository : IBaseRepository<WebhookEvent, WebhookEventId>
{
    Task<int> SaveChangesAsync();
}
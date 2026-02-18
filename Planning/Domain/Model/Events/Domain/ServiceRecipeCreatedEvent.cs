using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record ServiceRecipeCreatedEvent(
    Guid RecipeId,
    Guid CatalogId,
    Guid TechnicianId,
    string ServiceName,
    string ServiceCategory,
    decimal TotalPrice,
    string Currency,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}


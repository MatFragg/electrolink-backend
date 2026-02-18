using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;

public record ServiceRecipeDeactivatedEvent(
    Guid RecipeId,
    Guid CatalogId,
    string Reason,
    DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}


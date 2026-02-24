using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using PropertyId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.PropertyId;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Events;

public record PropertyAddedToPortfolioEvent(PropertyPortfolioId PropertyPortfolioId, PropertyId PropertyId, string Nickname, bool IsPrimary, EPortfolioStatus PortfolioStatus, DateTime OccurredOn
) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using PropertyId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.PropertyId;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class PortfolioEntry
{
    public PortfolioEntryId Id { get; private set; } = null!;
    public PropertyPortfolioId PortfolioId { get; private set; } = null!;
    public PropertyId PropertyId { get; private set; } = null!;
    public string Nickname { get; private set; } = null!;
    public bool IsPrimary { get; private set; }
    public EOccupancyStatus OccupancyStatus { get; private set; }

    private PortfolioEntry() { }

    public static PortfolioEntry Create(
        PropertyPortfolioId portfolioId,
        PropertyId propertyId,
        string nickname,
        bool isPrimary,
        EOccupancyStatus occupancyStatus)
    {
        return new PortfolioEntry
        {
            Id = PortfolioEntryId.NewPortfolioEntryId(),
            PortfolioId = portfolioId,
            PropertyId = propertyId,
            Nickname = nickname,
            IsPrimary = isPrimary,
            OccupancyStatus = occupancyStatus,
        };
    }

    internal void SetNonPrimary() => IsPrimary = false;
}
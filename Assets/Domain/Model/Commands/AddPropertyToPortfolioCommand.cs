using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddPropertyToPortfolioCommand(HomeownerId HomeownerId, PropertyId PropertyId, string Nickname, bool IsPrimary, string OccupancyStatus);
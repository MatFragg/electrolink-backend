using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RemovePropertyFromPortfolioCommand(HomeownerId HomeownerId, PropertyId PropertyId, string Reason);

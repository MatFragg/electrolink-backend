using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CreatePlanCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    MonetizationType MonetizationType, // "Monthly", "Yearly", "Lifetime"
    bool IsDefault
);
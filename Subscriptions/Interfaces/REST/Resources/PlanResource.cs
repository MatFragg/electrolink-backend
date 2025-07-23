namespace Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record PlanResource(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault
);
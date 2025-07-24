namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record UpdatePlanResource(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault
);
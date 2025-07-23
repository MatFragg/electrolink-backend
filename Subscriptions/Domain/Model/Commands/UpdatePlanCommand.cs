namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record UpdatePlanCommand(
    Guid PlanId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string MonetizationType,
    bool IsDefault
);
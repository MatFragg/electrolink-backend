namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record SubscriptionId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
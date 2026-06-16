namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record ConsumptionDashboardId
{
    public string Value { get; }
    private ConsumptionDashboardId(string value) => Value = value;
    public static ConsumptionDashboardId New() =>
        new($"dash-{Guid.NewGuid()}");
    public static ConsumptionDashboardId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ConsumptionDashboardId cannot be empty.");
        return new ConsumptionDashboardId(value);
    }
}

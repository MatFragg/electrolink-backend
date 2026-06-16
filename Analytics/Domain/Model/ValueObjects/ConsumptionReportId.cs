namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record ConsumptionReportId
{
    public string Value { get; }
    private ConsumptionReportId(string value) => Value = value;
    public static ConsumptionReportId New() =>
        new($"rpt-{Guid.NewGuid()}");
    public static ConsumptionReportId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ConsumptionReportId cannot be empty.");
        return new ConsumptionReportId(value);
    }
}

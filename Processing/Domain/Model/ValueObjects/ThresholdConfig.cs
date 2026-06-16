namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public record ThresholdConfig
{
    public float  NominalVoltage           { get; }
    public float  MaxConsumptionWatts      { get; }
    public float  MaxCurrentAmps           { get; }
    public float  MinPowerFactor           { get; }
    public float  NominalFrequency         { get; }
    public int    DisconnectionThresholdMin { get; }

    private ThresholdConfig(
        float nominalVoltage, float maxConsumptionWatts,
        float maxCurrentAmps, float minPowerFactor,
        float nominalFrequency, int disconnectionThresholdMin)
    {
        NominalVoltage            = nominalVoltage;
        MaxConsumptionWatts       = maxConsumptionWatts;
        MaxCurrentAmps            = maxCurrentAmps;
        MinPowerFactor            = minPowerFactor;
        NominalFrequency          = nominalFrequency;
        DisconnectionThresholdMin = disconnectionThresholdMin;
    }

    public static ThresholdConfig Default() =>
        new(220f, 5000f, 20f, 0.85f, 60f, 10);

    public static ThresholdConfig Create(
        float nominalVoltage, float maxConsumptionWatts,
        float maxCurrentAmps, float minPowerFactor,
        float nominalFrequency, int disconnectionThresholdMin)
    {
        if (nominalVoltage <= 0)
            throw new ArgumentException("Nominal voltage must be positive.");
        if (maxConsumptionWatts <= 0)
            throw new ArgumentException("Max consumption must be positive.");
        if (minPowerFactor < 0 || minPowerFactor > 1)
            throw new ArgumentException("Power factor must be between 0 and 1.");
        if (disconnectionThresholdMin < 1)
            throw new ArgumentException("Disconnection threshold must be at least 1 minute.");

        return new ThresholdConfig(
            nominalVoltage, maxConsumptionWatts,
            maxCurrentAmps, minPowerFactor,
            nominalFrequency, disconnectionThresholdMin);
    }
}

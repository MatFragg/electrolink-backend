namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public enum EAnomalyType
{
    VoltageSpike,
    VoltageSag,
    SustainedOverconsumption,
    ShortCircuitRisk,
    PowerFactorDegradation,
    AbnormalFrequency,
    ConnectionLost,
    UnusualConsumptionPattern
}

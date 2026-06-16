namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record UpdateCustomThresholdsCommand(
    string  HomeownerId,
    float   NominalVoltage,
    float   MaxConsumptionWatts,
    float   MaxCurrentAmps,
    float   MinPowerFactor,
    float   NominalFrequency,
    int     DisconnectionThresholdMin
);

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource for recording a maintenance event on a property.
/// </summary>
public record RecordMaintenanceResource(
    string ServiceId,
    string TechnicianId,
    string WorkSummary,
    DateTime CompletedAt);


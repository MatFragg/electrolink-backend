using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

public record IoTDeviceResource(
    string Id,
    string SerialNumber,
    string FirmwareVersion,
    EDeviceStatus Status,
    string? AssignedPropertyId,
    EConnectionStatus ConnectionStatus,
    DateTime? LastReadingAt,
    DateTime? InstalledAt);

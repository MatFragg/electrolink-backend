using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class RecordInstallationCommandFromResourceAssembler
{
    public static RecordDeviceInstallationCommand ToCommandFromResource(
        string deviceId, string propertyId, RecordDeviceInstallationResource resource)
        => new(
            deviceId,
            propertyId,
            resource.TechnicianId,
            null,
            resource.FirmwareVersion,
            resource.InstalledAt);
}

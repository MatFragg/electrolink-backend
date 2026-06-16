using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class RegisterIoTDeviceCommandFromResourceAssembler
{
    public static RegisterIoTDeviceCommand ToCommandFromResource(RegisterIoTDeviceResource resource)
        => new(
            resource.SerialNumber,
            resource.PlainTextApiKey,
            resource.FirmwareVersion);
}

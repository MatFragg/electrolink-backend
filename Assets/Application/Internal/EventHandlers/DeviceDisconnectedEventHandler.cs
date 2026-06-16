using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceDisconnectedEventHandler
    : INotificationHandler<DeviceDisconnectedExternalEvent>
{
    private readonly IIoTDeviceCommandService _commandService;

    public DeviceDisconnectedEventHandler(IIoTDeviceCommandService commandService)
        => _commandService = commandService;

    public Task Handle(DeviceDisconnectedExternalEvent notification, CancellationToken cancellationToken)
        => _commandService.Handle(new UpdateDeviceConnectionStatusCommand(
            DeviceId: notification.DeviceId,
            NewConnectionStatus: EConnectionStatus.Disconnected,
            LastReadingAt: notification.LastReadingAt));
}

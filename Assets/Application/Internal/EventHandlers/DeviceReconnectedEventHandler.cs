using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.EventHandlers;

public class DeviceReconnectedEventHandler
    : INotificationHandler<DeviceReconnectedExternalEvent>
{
    private readonly IIoTDeviceCommandService _commandService;

    public DeviceReconnectedEventHandler(IIoTDeviceCommandService commandService)
        => _commandService = commandService;

    public Task Handle(DeviceReconnectedExternalEvent notification, CancellationToken cancellationToken)
        => _commandService.Handle(new UpdateDeviceConnectionStatusCommand(
            DeviceId: notification.DeviceId,
            NewConnectionStatus: EConnectionStatus.Connected,
            LastReadingAt: notification.ReconnectedAt));
}

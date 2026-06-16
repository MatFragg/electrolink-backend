using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using Hampcoders.Electrolink.API.Processing.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Processing.Application.Internal.EventHandlers;

public class DeviceDecommissionedEventHandler(
    IDeviceReadingStreamRepository streamRepository,
    IUnitOfWork                    unitOfWork,
    ILogger<DeviceDecommissionedEventHandler> logger)
    : INotificationHandler<DeviceDecommissionedEvent>
{
    public async Task Handle(DeviceDecommissionedEvent notification, CancellationToken cancellationToken)
    {
        var stream = await streamRepository.FindByDeviceIdAsync(notification.DeviceId);
        if (stream is null) return;

        stream.Pause();
        await streamRepository.UpdateAsync(stream);
        await unitOfWork.CompleteAsync();

        logger.LogInformation("[IoT] DeviceReadingStream DECOMMISSIONED for device {DeviceId}.", notification.DeviceId);
    }
}

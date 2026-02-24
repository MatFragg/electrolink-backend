using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;

[Serializable]
public sealed class ReservationNotFoundException : Exception
{
    public ServiceId? ServiceId { get; }

    public ReservationNotFoundException()
        : base("Reservation not found.")
    {
    }

    public ReservationNotFoundException(ServiceId serviceId)
        : base($"Reservation for Service with id '{serviceId}' was not found.")
    {
        ServiceId = serviceId;
    }

    public ReservationNotFoundException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    private ReservationNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ServiceId = (ServiceId?)info.GetValue(nameof(ServiceId), typeof(ServiceId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(ServiceId), ServiceId, typeof(ServiceId));
        base.GetObjectData(info, context);
    }
}
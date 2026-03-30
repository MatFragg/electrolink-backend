using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;

[Serializable]
public sealed class ReservationNotFoundException : Exception
{
    public AssignmentId? AssignmentId { get; }

    public ReservationNotFoundException()
        : base("Reservation not found.")
    {
    }

    public ReservationNotFoundException(AssignmentId assignmentId)
        : base($"Reservation for Service with id '{assignmentId}' was not found.")
    {
        AssignmentId = assignmentId;
    }

    public ReservationNotFoundException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    private ReservationNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        AssignmentId = (AssignmentId?)info.GetValue(nameof(AssignmentId), typeof(AssignmentId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(AssignmentId), AssignmentId, typeof(AssignmentId));
        base.GetObjectData(info, context);
    }
}
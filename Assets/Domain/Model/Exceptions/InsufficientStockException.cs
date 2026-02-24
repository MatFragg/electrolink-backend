using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;

[Serializable]
public sealed class InsufficientStockException : Exception
{
    public ComponentId? ComponentId { get; }
    public int Requested { get; }
    public int Available { get; }

    public InsufficientStockException()
        : base("Insufficient stock.")
    {
    }

    public InsufficientStockException(ComponentId componentId, int requested, int available)
        : base($"Insufficient stock for component '{componentId}'. Requested: {requested}, Available: {available}.")
    {
        ComponentId = componentId;
        Requested = requested;
        Available = available;
    }

    public InsufficientStockException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    private InsufficientStockException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ComponentId = (ComponentId?)info.GetValue(nameof(ComponentId), typeof(ComponentId));
        Requested = info.GetInt32(nameof(Requested));
        Available = info.GetInt32(nameof(Available));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(ComponentId), ComponentId, typeof(ComponentId));
        info.AddValue(nameof(Requested), Requested);
        info.AddValue(nameof(Available), Available);
        base.GetObjectData(info, context);
    }
}
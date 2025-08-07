using MediatR;  

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

public interface IEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
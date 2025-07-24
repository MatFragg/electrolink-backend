using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class RequestStatus
{
    public ServiceStatus Status { get; private set; }
    public DateTime Timestamp { get; private set; }

    public RequestStatus(ServiceStatus status, DateTime timestamp)
    {
        Status = status;
        Timestamp = timestamp;
    }
}
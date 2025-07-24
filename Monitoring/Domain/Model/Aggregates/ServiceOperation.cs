using System;
using System.Collections.Generic;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public partial class ServiceOperation
{
    public Guid RequestId { get; private set; }
    public ServiceStatus CurrentStatus { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    
    public TechnicianId TechnicianId { get; private set; }

    public ICollection<RequestStatus> StatusHistory { get; private set; }

    public ServiceOperation()
    {
        RequestId = Guid.Empty;
        CurrentStatus = ServiceStatus.Scheduled;
        StartedAt = DateTime.UtcNow;
        StatusHistory = new List<RequestStatus>();
    }

    public ServiceOperation(Guid requestId)
    {
        RequestId = requestId;
        CurrentStatus = ServiceStatus.Scheduled;
        StartedAt = DateTime.UtcNow;
        StatusHistory = new List<RequestStatus> {
            new(ServiceStatus.Scheduled, DateTime.UtcNow)
        };
    }

    public ServiceOperation(CreateServiceOperationCommand command)
        : this(command.RequestId)
    {
        TechnicianId = new TechnicianId(command.TechnicianId);
    }
    
    public void ChangeStatus(ServiceStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(ServiceStatus), newStatus))
            throw new ArgumentException("Invalid status.", nameof(newStatus));

        CurrentStatus = newStatus;
        StatusHistory.Add(new RequestStatus(newStatus, DateTime.UtcNow));

        if (newStatus == ServiceStatus.Completed)
            CompletedAt = DateTime.UtcNow;
    }
}

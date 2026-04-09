using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class ExtendServiceWaitTimeCommandFromResourceAssembler
{
    public static ExtendServiceWaitTimeCommand ToCommandFromResource(string executionId, string homeownerId, ExtendWaitTimeResource resource)
    {
        return new ExtendServiceWaitTimeCommand(
            ServiceExecutionId.From(executionId),
            HomeownerId.From(homeownerId),
            resource.ExtendMinutes,
            System.DateTime.UtcNow
        );
    }
}


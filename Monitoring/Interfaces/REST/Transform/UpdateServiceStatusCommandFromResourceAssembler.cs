using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class UpdateServiceStatusCommandFromResourceAssembler
{
    public static UpdateServiceStatusCommand ToCommandFromResource(Guid id, UpdateServiceStatusResource res)
        => new(id, Enum.Parse<ServiceStatus>(res.NewStatus));
}
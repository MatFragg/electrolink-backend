using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class UpdateServiceStatusCommandFromResourceAssembler
{
    public static UpdateServiceStatusCommand ToCommandFromResource(Guid id, UpdateServiceStatusResource res)
        => new(id, Enum.Parse<ServiceStatus>(res.NewStatus));
}
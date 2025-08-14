using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using System;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Integration;

public record ProfileCreatedIntegrationEvent(
    int ProfileId, // Mismo ID que UserId de IAM
    string Username, // Username asociado
    string EmailAddress,
    string FullName,
    string Role, // String para ser agnóstico del enum interno de Profiles
    DateTime OccurredOn
) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
};

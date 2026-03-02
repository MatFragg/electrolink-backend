using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record ReleaseReservationCommand(TechnicianId TechnicianId, ServiceId ServiceId, string Reason);

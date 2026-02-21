using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;

public sealed class TechnicianDataUpdatedEvent : IEvent
{
    public Guid EventId { get; }

    public ProfileId ProfileId { get; }
    public IEnumerable<ESpecialty>? Specialties { get; }
    int? ExperienceYears { get; }
    public string? AboutMe { get; }
    public TechnicianId TechnicianId { get; }

    public DateTime OccurredOn { get; }

    public TechnicianDataUpdatedEvent(ProfileId profileId, IEnumerable<ESpecialty>? specialties, int? experienceYears, string? aboutMe, TechnicianId technicianId)
    {
        EventId = Guid.NewGuid();
        ProfileId = profileId;
        Specialties = specialties;
        ExperienceYears = experienceYears;
        AboutMe = aboutMe;
        TechnicianId = technicianId;
        OccurredOn = DateTime.UtcNow;
    }
}
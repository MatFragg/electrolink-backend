using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class TechnicianSpecialty
{
    public TechnicianId TechnicianId { get; private set; }

    public ESpecialty Specialty { get; private set; }

    public Technician Technician { get; private set; }

    private TechnicianSpecialty() { }

    public TechnicianSpecialty(TechnicianId technicianId, ESpecialty specialty)
    {
        TechnicianId = technicianId;
        Specialty = specialty;
    }
}
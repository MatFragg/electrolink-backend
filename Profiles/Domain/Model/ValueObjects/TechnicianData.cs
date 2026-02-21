using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public sealed record TechnicianData
{
    public IReadOnlyList<ESpecialty> Specialties { get; init; }
    public int ExperienceYears { get; init; }
    public string AboutMe { get; init; }

    private TechnicianData() { }

    public static TechnicianData Create(IEnumerable<ESpecialty> specialties, int experienceYears, string? aboutMe)
    {
        var list = specialties?.ToList() ?? new List<ESpecialty>();
        if (list.Count == 0)
            throw new AtLeastOneSpecialtyRequiredException();

        return new TechnicianData
        {
            Specialties = list.AsReadOnly(),
            ExperienceYears = experienceYears,
            AboutMe = aboutMe ?? string.Empty
        };
    }
}
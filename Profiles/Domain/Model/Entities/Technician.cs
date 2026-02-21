using System.ComponentModel.DataAnnotations;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class Technician
{
  [Key]
  public TechnicianId TechnicianId { get; private set; }
  public ProfileId ProfileId { get; private set; }

  public IReadOnlyList<ESpecialty> Specialties => _specialtyEntities.Select(e => e.Specialty).ToList().AsReadOnly();
  public int ExperienceYears { get; private set; }   
  public string AboutMe { get; private set; }

  private List<TechnicianSpecialty> _specialtyEntities = new();

  public static Technician Create(
    TechnicianId id,
    ProfileId profileId,
    IEnumerable<ESpecialty> specialties,
    int experienceYears,
    string aboutMe)
  {
    var specialtiesList = specialties?.ToList() ?? new List<ESpecialty>();
    if (specialtiesList.Count == 0)
      throw new AtLeastOneSpecialtyRequiredException();

    var technician = new Technician
    {
      TechnicianId    = id,
      ProfileId       = profileId,
      _specialtyEntities = specialtiesList.Select(s => new TechnicianSpecialty(id, s)).ToList(),
      ExperienceYears = experienceYears,
      AboutMe         = aboutMe ?? string.Empty,
    };
    return technician;
  }
  
  public void UpdateExperienceYears(int experienceYears)
  {
    ExperienceYears = experienceYears;
  }
  
  public void UpdateSpecialties(IEnumerable<ESpecialty> specialties)
  {
    var list = specialties?.ToList() ?? new List<ESpecialty>();
    if (list.Count == 0)
      throw new AtLeastOneSpecialtyRequiredException();

    _specialtyEntities = list.Select(s => new TechnicianSpecialty(TechnicianId, s)).ToList();
  }
  
  public void UpdateAboutMe(string? aboutMe)
  {
    AboutMe = aboutMe ?? string.Empty;
  }
}

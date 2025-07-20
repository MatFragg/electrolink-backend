using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class Technician
{
  [Key]
  public Guid Id { get; private set; }

  public string LicenseNumber { get; private set; }
  public string Specialization { get; private set; }

  protected Technician()
  {
    LicenseNumber = string.Empty;
    Specialization = string.Empty;
  }

  public Technician(string licenseNumber, string specialization)
  {
    LicenseNumber = licenseNumber;
    Specialization = specialization;
  }

  public void UpdateSpecialization(string specialization)
  {
    Specialization = specialization;
  }
}

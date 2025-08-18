using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class HomeOwner
{
  [Key] 
  public Guid Id { get; private set; }

  private readonly List<string> _properties = new();
  public IReadOnlyList<string> Properties => _properties.AsReadOnly();

  public bool IsVerified { get; private set; }
  public string Dni { get; private set; }
  public int ProfileId { get; private set; }

  protected HomeOwner()
  {
    Id = Guid.NewGuid();
    IsVerified = false;
    Dni = string.Empty;
    ProfileId = 0;
  }
  
  public HomeOwner(string dni) : this()
  {
    if (string.IsNullOrWhiteSpace(dni)) throw new ArgumentException("DNI cannot be null or empty.", nameof(dni));
    Dni = dni;
  }
  
  public HomeOwner(int profileId, string dni)
  {
    if (profileId <= 0) throw new ArgumentException("ProfileId must be a positive integer.", nameof(profileId));
    if (string.IsNullOrWhiteSpace(dni)) throw new ArgumentException("DNI cannot be null or empty.", nameof(dni));

    ProfileId = profileId;
    Dni = dni;
  }

  public void SetProfileId(int profileId)
  {
    if (ProfileId != 0) throw new InvalidOperationException("ProfileId has already been set.");
    ProfileId = profileId;
  }
  
  public void AddProperty(string propertyAddress)
  {
    if (!string.IsNullOrWhiteSpace(propertyAddress) && !_properties.Contains(propertyAddress))
      _properties.Add(propertyAddress);
  }

  public void RemoveProperty(string propertyAddress)
  {
    _properties.Remove(propertyAddress);
  }
  public void UpdateDni(string newDni)
  {
    if (string.IsNullOrWhiteSpace(newDni)) throw new ArgumentException("DNI cannot be null or empty.", nameof(newDni));
    Dni = newDni;
  }
  public void Verify()
  {
    IsVerified = true;
  }
}

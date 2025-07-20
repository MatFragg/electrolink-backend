using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class HomeOwner
{
  [Key] // Marca esta propiedad como clave primaria
  public Guid Id { get; private set; }

  private readonly List<string> _properties = new();
  public IReadOnlyList<string> Properties => _properties.AsReadOnly();

  public string PreferredContactTime { get; private set; }
  public bool IsVerified { get; private set; }
  public string Dni { get; private set; }

  protected HomeOwner()
  {
    PreferredContactTime = string.Empty;
    IsVerified = false;
    Dni = string.Empty;
  }

  public HomeOwner(string dni)
  {
    Dni = dni;
    PreferredContactTime = string.Empty;
    IsVerified = false;
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

  public void Verify()
  {
    IsVerified = true;
  }

  public void UpdatePreferredContactTime(string preferredContactTime)
  {
    PreferredContactTime = preferredContactTime;
  }
}

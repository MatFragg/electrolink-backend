using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class Technician
{
  [Key]
  public Guid Id { get; private set; }
  public int ProfileId { get; private set; }
  public string LicenseNumber { get; private set; } 
  public string Specialization { get; private set; }
  private readonly List<PortfolioItem> _portfolioItems = new();
  public IReadOnlyList<PortfolioItem> PortfolioItems => _portfolioItems.AsReadOnly();
  protected Technician()
  {
    Id = Guid.NewGuid();
    LicenseNumber = string.Empty;
    Specialization = string.Empty;
    ProfileId = 0;
  }

  public Technician(string licenseNumber, string specialization) : this() 
  {
    if (string.IsNullOrWhiteSpace(licenseNumber)) throw new ArgumentException("License Number cannot be null or empty.", nameof(licenseNumber));
    if (string.IsNullOrWhiteSpace(specialization)) throw new ArgumentException("Specialization cannot be null or empty.", nameof(specialization));
    LicenseNumber = licenseNumber;
    Specialization = specialization;
  }

  public Technician(int profileId, string licenseNumber, string specialization) : this() 
  {
    if (profileId <= 0) throw new ArgumentException("ProfileId must be a positive integer.", nameof(profileId));
    if (string.IsNullOrWhiteSpace(licenseNumber)) throw new ArgumentException("License Number cannot be null or empty.", nameof(licenseNumber));
    if (string.IsNullOrWhiteSpace(specialization)) throw new ArgumentException("Specialization cannot be null or empty.", nameof(specialization));

    ProfileId = profileId;
    LicenseNumber = licenseNumber;
    Specialization = specialization;
  }
  public void SetProfileId(int profileId)
  {
    if (ProfileId != 0) throw new InvalidOperationException("ProfileId has already been set.");
    ProfileId = profileId;
  }
  public void UpdateSpecialization(string specialization)
  {
    if (string.IsNullOrWhiteSpace(specialization)) throw new ArgumentException("Specialization cannot be null or empty.", nameof(specialization));
    Specialization = specialization;
  }
  
  public PortfolioItem AddPortfolioItem(string title, string description, string imageUrl)
  {
    var newPortfolioItem = new PortfolioItem(title, description, imageUrl, Id); 
    _portfolioItems.Add(newPortfolioItem);
    return newPortfolioItem;
  }

  public void UpdatePortfolioItemDetails(Guid workId, string newTitle, string newDescription, string newImageUrl)
  {
    var existingItem = _portfolioItems.FirstOrDefault(item => item.WorkId == workId);
    if (existingItem == null)
      throw new ArgumentException($"Portfolio item with WorkId {workId} not found.");

    existingItem.UpdateDetails(newTitle, newDescription, newImageUrl);
  }

  public void RemovePortfolioItem(Guid workId)
  {
    var itemToRemove = _portfolioItems.FirstOrDefault(item => item.WorkId == workId);
    if (itemToRemove == null)
      throw new ArgumentException($"Portfolio item with WorkId {workId} not found.");

    _portfolioItems.Remove(itemToRemove);
  }
}

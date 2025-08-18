using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;

public class PortfolioItem
{
    public Guid Id { get; private set; }
    public Guid WorkId { get; private set; } 
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public Guid TechnicianId { get; private set; }

    protected PortfolioItem()
    {
        Id = Guid.NewGuid(); 
        WorkId = Guid.Empty;
    }

    public PortfolioItem(Guid workId, string title, string description, string imageUrl, Guid technicianId)
    {
        if (workId == Guid.Empty) throw new ArgumentException("WorkId cannot be empty.", nameof(workId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        if (string.IsNullOrWhiteSpace(imageUrl)) throw new ArgumentException("ImageUrl cannot be null or empty.", nameof(imageUrl));
        if (technicianId == Guid.Empty) throw new ArgumentException("TechnicianId must be valid.", nameof(technicianId));

        WorkId = workId;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        TechnicianId = technicianId;
    }

    public PortfolioItem(string title, string description, string imageUrl, Guid technicianId)
        : this(Guid.NewGuid(), title, description, imageUrl, technicianId) { }
    
    public void UpdateDetails(string newTitle, string newDescription, string newImageUrl)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) throw new ArgumentException("Title cannot be null or empty.", nameof(newTitle));
        if (string.IsNullOrWhiteSpace(newImageUrl)) throw new ArgumentException("ImageUrl cannot be null or empty.", nameof(newImageUrl));

        Title = newTitle;
        Description = newDescription;
        ImageUrl = newImageUrl;
    }
}
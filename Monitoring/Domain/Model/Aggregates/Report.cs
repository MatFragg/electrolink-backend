using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class Report
{
    public Guid ReportId { get; private set; }
    public Guid RequestId { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    
    private List<ReportPhoto> _photos = new();
    public IReadOnlyCollection<ReportPhoto> Photos => _photos;

    // Constructor completo
    public Report(Guid reportId, Guid requestId, string description, DateTime date)
    {
        ReportId = reportId;
        RequestId = requestId;
        Description = description;
        Date = date;
    }

    // Constructor simplificado para crear un nuevo Report
    public Report(Guid requestId, string description)
        : this(Guid.NewGuid(), requestId, description, DateTime.UtcNow)
    {
    }
    
    public void SetPhotos(IEnumerable<ReportPhoto> photos)
    {
        _photos.Clear();
        _photos.AddRange(photos);
    }
}

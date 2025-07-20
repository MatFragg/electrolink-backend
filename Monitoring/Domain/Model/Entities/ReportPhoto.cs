using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class ReportPhoto
{
    public Guid Id { get; private set; }
    public Guid ReportId { get; private set; }
    public string Url { get; private set; }
    public string Type { get; private set; }

    public ReportPhoto(Guid reportId, string url, string type)
    {
        Id = Guid.NewGuid(); 
        ReportId = reportId;
        Url = url;
        Type = type;
    }
    
    protected ReportPhoto() { }
}

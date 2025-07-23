namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;

public class ServiceDocument
{
    public string DocumentId { get; private set; }
    public string ServiceId { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }

    public ServiceDocument(string documentId, string serviceId, string name, string url)
    {
        DocumentId = documentId;
        ServiceId = serviceId;
        Name = name;
        Url = url;
    }
}
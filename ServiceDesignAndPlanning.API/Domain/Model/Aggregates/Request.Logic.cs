using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;

public partial class Request
{
    public void UpdateScheduledDate(DateOnly newDate) =>
        ScheduledDate = newDate;

    public void Cancel()
    {
        if (Status != "Cancelled")
            Status = "Cancelled";
    }

    public void AssignTechnician(Guid technicianId)
    {
        TechnicianId = technicianId;
        Status = "Confirmed";
    }

    public void AddPhoto(string photoId, string url)
    {
        if (!Photos.Any(p => p.PhotoId == photoId))
            Photos.Add(new RequestPhoto(photoId, url));
    }

    public void ChangeStatus(string newStatus)
    {
        Status = newStatus;
    }
}
using System.Linq;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class RequestResourceFromEntityAssembler
{
    public static RequestResource ToResourceFromEntity(Request r) =>
        new RequestResource(
            r.Id.ToString(),
            r.ClientId.ToString(),
            r.TechnicianId.ToString(),
            r.PropertyId.ToString(),
            r.ServiceId.ToString(),
            r.ProblemDescription,
            r.ScheduledDate,
            r.Status.ToString(),
            new ElectricBill(
                r.Bill.BillingPeriod,
                r.Bill.EnergyConsumed,
                r.Bill.AmountPaid,
                r.Bill.BillImageUrl
            ),
            r.Photos.Select(p => new RequestPhotoResource(p.PhotoId, p.Url)).ToList()
        );
}
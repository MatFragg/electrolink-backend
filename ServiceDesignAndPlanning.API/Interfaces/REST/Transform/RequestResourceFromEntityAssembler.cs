using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

public static class RequestResourceFromEntityAssembler
{
    public static RequestResource ToResourceFromEntity(Request r) =>
        new RequestResource(
            r.RequestId,
            r.ClientId.ToString(),
            r.TechnicianId.ToString(),
            r.PropertyId.ToString(),
            r.ServiceId.ToString(),
            r.ProblemDescription,
            r.ScheduledDate,
            r.Status,
            new ElectricBill(
                r.Bill.BillingPeriod,
                r.Bill.EnergyConsumed,
                r.Bill.AmountPaid,
                r.Bill.BillImageUrl
            ),
            r.Photos.Select(p => new RequestPhotoResource(p.PhotoId, p.Url)).ToList()
        );
}
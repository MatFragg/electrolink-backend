using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

public static class CreateRequestCommandFromResourceAssembler
{
    public static CreateRequestCommand ToCommandFromResource(CreateRequestResource r) =>
        new CreateRequestCommand(
            r.ClientId,
            r.TechnicianId,
            r.PropertyId,
            r.ServiceId,
            "Pending", // Si deseas parametrizar el estado, agrégalo al Resource
            r.ScheduledDate,
            r.ProblemDescription,
            new ElectricBill(
                r.Bill.BillingPeriod,
                r.Bill.EnergyConsumed,
                r.Bill.AmountPaid,
                r.Bill.BillImageUrl
            )
        );
}
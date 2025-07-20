using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;

public partial class Request
{
    public string RequestId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid TechnicianId { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid ServiceId { get; private set; }
    public string Status { get; private set; }
    public DateOnly ScheduledDate { get; private set; }
    public string ProblemDescription { get; set; }

    public List<RequestPhoto> Photos { get; private set; } = new();
    public ElectricBill Bill { get; private set; }

    public Request()
    {
        Status = "Pending";
        ProblemDescription = string.Empty;
    }

    public Request(CreateRequestCommand command)
    {
        RequestId = Guid.NewGuid().ToString(); // ← ID autogenerado internamente
        ClientId = command.ClientId;
        TechnicianId = command.TechnicianId;
        PropertyId = command.PropertyId;
        ServiceId = command.ServiceId;
        Status = command.Status ?? "Pending";
        ScheduledDate = command.ScheduledDate;
        ProblemDescription = command.ProblemDescription;
        Bill = command.Bill;
    }
    
}
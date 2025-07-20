namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;

public class ServicePlan
{
    public string PlanId { get; private set; }
    public string ServiceId { get; private set; }
    public string Name { get; private set; }
    public double AdditionalPrice { get; private set; }
    public string EstimatedExtraTime { get; private set; }

    public ServicePlan(string planId, string serviceId, string name, double additionalPrice, string estimatedExtraTime)
    {
        PlanId = planId;
        ServiceId = serviceId;
        Name = name;
        AdditionalPrice = additionalPrice;
        EstimatedExtraTime = estimatedExtraTime;
    }
}
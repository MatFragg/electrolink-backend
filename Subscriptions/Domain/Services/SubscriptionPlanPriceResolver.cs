using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public class SubscriptionPlanPriceResolver
{
    private readonly IConfiguration _configuration;

    public SubscriptionPlanPriceResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ResolvePriceId(PlanType planType, BillingCycle billingCycle)
    {
        if (planType.IsBasic)
            throw new InvalidOperationException("Basic plan does not have a Stripe price ID.");

        var key = (billingCycle.Value) switch
        {
            EBillingCycle.Monthly => "Stripe:Prices:TechnicianMonthly",
            EBillingCycle.Annual => "Stripe:Prices:TechnicianAnnual",
            _ => throw new ArgumentException("Invalid billing cycle.")
        };

        return _configuration[key] ?? throw new InvalidOperationException($"Stripe price is not configured for key '{key}'.");
    }

    public string ResolvePriceIdForRole(EBusinessRole role, EBillingCycle billingCycle)
    {
        var key = (role, billingCycle) switch
        {
            (EBusinessRole.Technician, EBillingCycle.Monthly) => "Stripe:Prices:TechnicianMonthly",
            (EBusinessRole.Technician, EBillingCycle.Annual) => "Stripe:Prices:TechnicianAnnual",
            (EBusinessRole.Homeowner, EBillingCycle.Monthly) => "Stripe:Prices:HomeownerMonthly",
            (EBusinessRole.Homeowner, EBillingCycle.Annual) => "Stripe:Prices:HomeownerAnnual",
            _ => throw new ArgumentException("Invalid role/cycle combination.")
        };

        return _configuration[key] ?? throw new InvalidOperationException($"Stripe price is not configured for key '{key}'.");
    }
}

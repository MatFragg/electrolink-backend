using System;
using System.Linq;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Transform;

public static class CreateServiceCommandFromResourceAssembler
{
    public static CreateServiceCommand ToCommandFromResource(CreateServiceResource r) =>
        new CreateServiceCommand(
            Guid.NewGuid().ToString(),
            r.Name,
            r.Description,
            r.BasePrice,
            r.EstimatedTime,
            r.Category,
            r.IsVisible,
            r.CreatedBy,
            new ServicePolicy(r.Policy.CancellationPolicy, r.Policy.TermsAndConditions),
            new ServiceRestriction(r.Restriction.UnavailableDistricts, r.Restriction.ForbiddenDays, r.Restriction.RequiresSpecialCertification),
            r.Tags.Select(t => new ServiceTag(t.Name)).ToList(),
            r.Components.Select(c => new ServiceComponent(c.ComponentId, c.Quantity)).ToList()
        );
}
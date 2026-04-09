using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

/// <summary>
/// Query to retrieve the active/current service execution for a homeowner.
/// </summary>
public record GetActiveServiceByHomeownerQuery(HomeownerId HomeownerId);



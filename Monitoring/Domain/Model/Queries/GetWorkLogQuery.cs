using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

/// <summary>
/// Query to retrieve the work log (photos, components used, reports) for a service execution.
/// </summary>
public record GetWorkLogQuery(ServiceExecutionId ExecutionId);



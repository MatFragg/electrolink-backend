using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

/// <summary>
/// Query to retrieve a service execution by its ID.
/// </summary>
public record GetServiceExecutionByIdQuery(ServiceExecutionId ExecutionId);



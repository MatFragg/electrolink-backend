using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

/// <summary>
/// Query to retrieve all service executions assigned to a specific technician.
/// </summary>
public record GetAssignedServicesByTechnicianQuery(TechnicianId TechnicianId);



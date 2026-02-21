namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;

/// <summary>
/// Punto de acceso que Monitoring ofrece a otras capas/BC
/// para crear y consultar ServiceOperations ligadas a Requests.
/// </summary>
public interface IMonitoringContextFacade
{
    /// <summary>
    /// Crea una <see cref="Domain.Model.Aggregates.ServiceOperation"/>
    /// vinculada al <paramref name="requestId"/> que viene del BC
    /// Service Design and Planning.
    /// </summary>
    /// <param name="requestId">Identificador del Request (GUID en SDP).</param>
    /// <param name="technicianId">Técnico responsable.</param>
    /// <returns>Guid que identifica la ServiceOperation creada.</returns>
    Task<Guid> CreateServiceOperationForRequestAsync(Guid requestId, string technicianId);

}
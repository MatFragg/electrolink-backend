using Hampcoders.Electrolink.API.Monitoring.Application.ACL;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[SwaggerTag("Service Operation endpoints")]
public class ServiceOperationsController(
    IServiceOperationCommandService commandService,
    IServiceOperationQueryService queryService, IMonitoringContextFacade monitoringContextFacade,
    IServiceOperationRepository repository
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Service Operation",
        Description = "Creates a new service operation using a valid external RequestId from ServiceDesignAndPlanning.",
        OperationId = "CreateServiceOperation")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created", typeof(ServiceOperationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid RequestId or input")]
    public async Task<IActionResult> CreateServiceOperation([FromBody] CreateServiceOperationResource resource)
    {
        try
        {
            // Usa el ACL en vez del CommandService directo
            var requestId = await monitoringContextFacade.CreateServiceOperationForRequestAsync(resource.RequestId, resource.TechnicianId);

            // Puedes consultar la entidad completa desde el repositorio si deseas retornar su representación
            var entity = await repository.FindByIdAsync(requestId); // <- necesitas este método en el repo
            if (entity == null) return NotFound();

            var resourceResult = ServiceOperationResourceFromEntityAssembler.ToResourceFromEntity(entity);
            return CreatedAtAction(nameof(GetStatusById), new { id = requestId }, resourceResult);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    [SwaggerOperation(Summary = "Update status", Description = "Updates the current status of the service operation.", OperationId = "UpdateServiceOperationStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated", typeof(string))]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateServiceStatusResource resource)
    {
        var command = new UpdateServiceStatusCommand(id, Enum.Parse<ServiceStatus>(resource.NewStatus));
        var updated = await commandService.Handle(command);
        return Ok(updated?.CurrentStatus.ToString());
    }

    // Obtener estado
    [HttpGet("{id}/status")]
    [SwaggerOperation(Summary = "Get status", Description = "Gets the current status of the service operation.", OperationId = "GetServiceOperationStatusById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Status retrieved", typeof(object))]
    public async Task<IActionResult> GetStatusById(Guid id)
    {
        var query = new GetServiceStatusByIdQuery(id);
        var result = await queryService.Handle(query);
        if (result == null) return NotFound();
        return Ok(new { code = (int)result.CurrentStatus, label = result.CurrentStatus.ToString() });
    }
    

    // Historial del técnico
    [HttpGet("technician/{technicianId}/history")]
    [SwaggerOperation(Summary = "Technician history", Description = "Gets the service operations completed by a technician.", OperationId = "GetClientHistory")]
    [SwaggerResponse(StatusCodes.Status200OK, "History retrieved", typeof(IEnumerable<ServiceOperation>))]
    public async Task<IActionResult> GetClientHistory(int technicianId)
    {
        var query = new GetClientHistoryByTechnicianIdQuery(technicianId);
        var history = await queryService.Handle(new GetClientHistoryByTechnicianIdQuery(technicianId));
        return Ok(history);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all service operations",
        Description = "Returns a list of all service operations in the system.",
        OperationId = "GetAllServiceOperations")]
    [SwaggerResponse(200, "List of service operations returned successfully")]
    public async Task<IActionResult> GetAllServiceOperations() {
        var result = await queryService.Handle(new GetAllServiceOperationsQuery());
        return Ok(result);
    }
}

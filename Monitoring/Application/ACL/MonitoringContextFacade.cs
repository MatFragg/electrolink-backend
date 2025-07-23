using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.ACL;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Application.ACL;


/// <inheritdoc />
public sealed class MonitoringContextFacade(
    ISDPContextFacade                 sdpFacade,
    IServiceOperationCommandService   operationCmdService,
    IServiceOperationRepository serviceOperationRepository,
    IProfilesContextFacade profilesContextFacade,   
    IUnitOfWork                       unitOfWork)
    : IMonitoringContextFacade
{
    public async Task<Guid> CreateServiceOperationForRequestAsync(Guid requestId, int technicianId)
    {
        // 1) Verificar que el Request exista en SDP.
        var requestDto = await sdpFacade.FetchRequestDetailsAsync(requestId.ToString());
        if (requestDto is null)
            throw new ArgumentException($"Request {requestId} not found in Service Design and Planning.");

        // 2) Verificar si ya existe una ServiceOperation para ese Request
        var existingOperation = await serviceOperationRepository.FindByIdAsync(requestId);
        if (existingOperation is not null)
            throw new InvalidOperationException($"A ServiceOperation already exists for Request {requestId}.");

        // 3) Verificar si existe un perfil con ese userId y que sea Technician
        var technicianExists = await profilesContextFacade.ExistsTechnicianProfileByUserIdAsync(technicianId);
        if (!technicianExists)
            throw new InvalidOperationException($"Technician profile not found for userId: {technicianId}");

        // 4) Crear nueva ServiceOperation
        var operation = await operationCmdService.Handle(new CreateServiceOperationCommand(requestId, technicianId));
        await unitOfWork.CompleteAsync();

        return operation.RequestId;
    }



}
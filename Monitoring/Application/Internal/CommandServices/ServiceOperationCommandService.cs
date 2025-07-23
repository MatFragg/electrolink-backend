using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;

public class ServiceOperationCommandService(
    IServiceOperationRepository repo,
    IUnitOfWork unitOfWork) : IServiceOperationCommandService
{
    public async Task<ServiceOperation?> Handle(CreateServiceOperationCommand command)
    {
        var operation = new ServiceOperation(command);
        await repo.AddAsync(operation);
        await unitOfWork.CompleteAsync();
        return operation;
    }

    public async Task<ServiceOperation?> Handle(UpdateServiceStatusCommand command)
    {
        var op = await repo.FindByIdAsync(command.RequestId);
        if (op is null) throw new ArgumentException($"ServiceOperation with ID {command.RequestId} not found.");

        op.Handle(command);
        await unitOfWork.CompleteAsync();
        return op;
    }
}

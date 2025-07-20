using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IServiceOperationCommandService
{
    Task<ServiceOperation?> Handle(CreateServiceOperationCommand command);
    Task<ServiceOperation?> Handle(UpdateServiceStatusCommand command);
    
}
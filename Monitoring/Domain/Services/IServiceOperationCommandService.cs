using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IServiceOperationCommandService
{
    Task<ServiceOperation?> Handle(CreateServiceOperationCommand command);
    Task<ServiceOperation?> Handle(UpdateServiceStatusCommand command);
    
}
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceCommandService
{
    Task<Service> Handle(CreateServiceCommand command);
    Task<Service?> Handle(UpdateServiceCommand command);
    Task<bool> Handle(DeleteServiceCommand command);
}
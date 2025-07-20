using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IServiceCommandService
{
    Task<Service> Handle(CreateServiceCommand command);
    Task<Service?> UpdateAsync(UpdateServiceCommand command);
    Task<bool> DeleteAsync(DeleteServiceCommand command);
}
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IServiceCommandService
{
    Task<Service> Handle(CreateServiceCommand command);
    Task<Service?> UpdateAsync(UpdateServiceCommand command);
    Task<bool> DeleteAsync(DeleteServiceCommand command);
}
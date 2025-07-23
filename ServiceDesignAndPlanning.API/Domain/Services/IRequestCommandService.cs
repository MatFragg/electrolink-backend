using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IRequestCommandService
{
    Task<Request> Handle(CreateRequestCommand command);
    Task<Request?> UpdateAsync(UpdateRequestCommand command);
    Task<bool> DeleteAsync(DeleteRequestCommand command);
}
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IRequestCommandService
{
    Task<Request> Handle(CreateRequestCommand command);
    Task<Request?> UpdateAsync(UpdateRequestCommand command);
    Task<bool> DeleteAsync(DeleteRequestCommand command);
}
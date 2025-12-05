using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IRequestCommandService
{
    Task<Request> Handle(CreateRequestCommand command);
    Task<Request?> UpdateAsync(UpdateRequestCommand command);
    Task<bool> DeleteAsync(DeleteRequestCommand command);
}
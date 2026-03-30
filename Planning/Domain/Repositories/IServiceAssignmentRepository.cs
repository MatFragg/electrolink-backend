using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceAssignmentRepository : IBaseRepository<ServiceAssignment, AssignmentId>
{
    Task<ServiceAssignment?> FindByRequestIdAsync(RequestId requestId);
}


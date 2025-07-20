using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Services;

/// <summary>
/// Query service interface for retrieving profiles.
/// </summary>
public interface IProfileQueryService
{
  Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query);

  Task<IEnumerable<Profile>> Handle(GetProfilesByRoleQuery query);

  Task<Profile?> Handle(GetProfileByIdQuery query);

  Task<Profile?> Handle(GetProfileByEmailQuery query);
}

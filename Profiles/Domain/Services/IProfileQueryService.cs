using Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Services;

/// <summary>
/// Query service interface for retrieving profiles.
/// </summary>
public interface IProfileQueryService
{
  Task<Profile?> Handle(GetProfileByIdQuery query);
  Task<Profile?> Handle(GetProfileByEmailQuery query);
  Task<Profile?> Handle(GetProfileInfoByUserIdQuery query);
  Task<MyProfileReadModel?>Handle(GetMyProfileQuery query);
  Task<ProfileStatusReadModel?>Handle(GetProfileStatusQuery query);
  Task<bool>Handle(IsHomeownerActiveQuery query);
}

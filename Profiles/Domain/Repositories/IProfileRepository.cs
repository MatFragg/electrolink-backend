using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Repositories;

/// <summary>
/// Repository interface for managing Profile aggregate roots.
/// </summary>
public interface IProfileRepository : IBaseRepository<Profile, ProfileId>
{
  /// <summary>
  /// Finds a profile by the associated IAM user ID.
  /// </summary>
  /// <param name="userId">The IAM user ID to search for.</param>
  /// <returns>The <see cref="Profile"/> if found, otherwise null.</returns>
  Task<Profile?> FindByUserIdAsync(UserId userId);

  /// <summary>
  /// Checks if a profile exists for the given IAM user ID.
  /// </summary>
  /// <param name="userId">The IAM user ID to check for existence.</param>
  /// <returns></returns>
  Task<bool>ExistsByUserIdAsync(UserId userId);

  /// <summary>
  /// Checks if a DNI is already associated with a profile, excluding a specific profile ID if provided.
  /// </summary>
  /// <param name="dni">The DNI to check for existence.</param>
  /// <param name="excludeProfileId">The profile ID to exclude from the check, if any.</param>
  /// <returns>True if the DNI exists, otherwise false.</returns>
  Task<bool>DniExistsAsync(Dni dni, ProfileId? excludeProfileId = null);
  
  /// <summary>
  /// Checks if the homeowner associated with the given homeowner ID is active.
  /// </summary>
  /// <param name="homeownerId"></param>
  /// <returns></returns>
  Task<bool>IsHomeownerActiveAsync(HomeownerId homeownerId);
  
  Task<IEnumerable<Profile>> FindByRoleAsync(EBusinessRole role);
  
  Task<IEnumerable<(string technicianId, string profileId, string fullName, double rating)>> FindTechniciansInAreaAsync(double lat, double lon);
  Task<(string ProfileId, string ProfileStatus, string? BusinessRole, string? RoleSubjectId)?> FindProfileClaimsByUserIdAsync(UserId userId);
}

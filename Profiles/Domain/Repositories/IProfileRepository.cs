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
  /// Finds a profile by email.
  /// </summary>
  /// <param name="email"></param>
  /// <returns></returns>
  Task<Profile?> FindByEmailAsync(Email email);
  /// <summary>
  /// Finds a profile by the associated IAM user ID.
  /// </summary>
  /// <param name="userId">The IAM user ID to search for.</param>
  /// <returns>The <see cref="Profile"/> if found, otherwise null.</returns>
  Task<Profile?> FindByUserIdAsync(UserId userId);

  /// <summary>
  /// Finds a profile by the associated IAM user ID, throwing an exception if not found.
  /// </summary>
  /// <param name="userId">The IAM user ID to search for.</param>
  /// <returns>The <see cref="Profile"/> if found.</returns>
  /// <exception cref="ProfileNotFoundException">Thrown if no profile is found for the given user ID.</exception>
  /// <remarks>Use this method when the existence of a profile is required for the operation to proceed.</remarks>
  Task<Profile>  FindByUserIdOrFailAsync(UserId userId);

  /// <summary>
  /// Checks if a profile exists for the given IAM user ID.
  /// </summary>
  /// <param name="userId">The IAM user ID to check for existence.</param>
  /// <returns></returns>
  Task<bool>ExistsByUserIdAsync(UserId userId);
  
  /// <summary>
  /// Checks if an email is already associated with a profile, excluding a specific profile ID if provided.
  /// </summary>
  /// <param name="email">The email to check for existence.</param>
  /// <param name="excludeProfileId">The profile ID to exclude from the check, if any.</param>
  /// <returns>True if the email exists, otherwise false.</returns>
  Task<bool>EmailExistsAsync(Email email, ProfileId? excludeProfileId = null);
  
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
}

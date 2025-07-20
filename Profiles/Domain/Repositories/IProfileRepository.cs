using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Repositories;

/// <summary>
/// Repository interface for managing Profile aggregate roots.
/// </summary>
public interface IProfileRepository : IBaseRepository<Profile>
{
  /// <summary>
  /// Finds a profile by email address.
  /// </summary>
  /// <param name="email">The email address to search for.</param>
  /// <returns>The <see cref="Profile"/> if found, otherwise null.</returns>
  Task<Profile?> FindByEmailAsync(string email);

  /// <summary>
  /// Finds all profiles by a specific role.
  /// </summary>
  /// <param name="role">The role to filter by (e.g., Homeowner or Technician).</param>
  /// <returns>A list of profiles matching the role.</returns>
  Task<IEnumerable<Profile>> FindByRoleAsync(Role role);
  
  Task<Profile?> FindByProfileIdAsync(int id);
  
  Task<IEnumerable<Profile>> ListWithDetailsAsync();
  
}

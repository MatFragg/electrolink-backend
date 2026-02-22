using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.IAM.Domain.Repositories;

/**
 * <summary>
 *     The user repository
 * </summary>
 * <remarks>
 *     This repository is used to manage users
 * </remarks>
 */
public interface IUserRepository : IBaseRepository<User, UserId>
{
    /**
     * <summary>
     *     Find a user by id
     * </summary>
     * <param name="email">The username to search</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByEmailAsync(string email);

    /**
     * <summary>
     *     Check if a user exists by username
     * </summary>
     * <param name="email">The username to search</param>
     * <returns>True if the user exists, false otherwise</returns>
     */
    bool ExistsByEmail(string email);
}
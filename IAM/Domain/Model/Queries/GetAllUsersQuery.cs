namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Queries;

/**
 * <summary>
 *     The get all users query
 * </summary>
 * <remarks>
 *     This query object is used to get all users with pagination
 * </remarks>
 */
public record GetAllUsersQuery(int Page = 1, int PageSize = 10);
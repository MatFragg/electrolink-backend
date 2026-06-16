namespace Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;

/**
 * <summary>
 *     Hashed password value object
 * </summary>
 * <remarks>
 *     This value object wraps a password hash string to provide type safety
 *     and prevent primitive obsession. It has no dependency on hashing infrastructure.
 * </remarks>
 */
public record HashedPassword
{
    public string Value { get; }

    private HashedPassword(string value) => Value = value;

    /**
     * <summary>
     *     Create a HashedPassword from an existing hash string
     * </summary>
     * <param name="hash">The hash string (e.g. from BCrypt)</param>
     * <returns>A new HashedPassword instance</returns>
     */
    public static HashedPassword FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));

        return new HashedPassword(hash);
    }
}

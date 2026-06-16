# Runtime Verification

- After any code changes, build with `dotnet build` (no errors).
- Run the app with `dotnet run --project Electrolink.API` and check startup completes (no EF Core/DI crashes).
- Test the `complete-homeowner` flow by sending a POST to `/api/v1/profiles/complete-homeowner` with appropriate JWT — verify it no longer fails with `InvalidIdException` for ProfileId.

# Known Naming / Edge Cases

- JWT `sub` claim holds a **user ID** (`us-...`), not a profile ID (`prof-...`). When looking up a profile by user, first resolve via `GetProfileClaimsAsync(userId)` to get the `ProfileId`, then use profile-ID-based methods.

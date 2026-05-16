using Microsoft.AspNetCore.Http;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UploadProfilePictureCommand(string ProfileId, IFormFile File);

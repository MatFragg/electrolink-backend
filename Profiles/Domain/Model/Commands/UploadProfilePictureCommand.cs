namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UploadProfilePictureCommand(string ProfileId, string UserId, Stream FileStream, string FileName);

using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.ExternalProviders;

public class CloudinarySettings
{
    public const string SectionName = "Cloudinary";

    [Required(ErrorMessage = "Cloudinary CloudName is required")]
    public string CloudName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cloudinary ApiKey is required")]
    public string ApiKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cloudinary ApiSecret is required")]
    public string ApiSecret { get; set; } = string.Empty;
}

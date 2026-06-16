namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;

public class AssetNotFoundException : Exception
{
    public string AssetType { get; }
    public string AssetId { get; }

    public AssetNotFoundException(string assetType, string id)
        : base($"{assetType} with ID {id} was not found.")
    {
        AssetType = assetType;
        AssetId = id;
    }
}

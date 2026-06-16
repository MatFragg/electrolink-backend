namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;

public class DuplicateAssetException : Exception
{
    public string AssetType { get; }
    public string Criteria { get; }

    public DuplicateAssetException(string assetType, string criteria)
        : base($"{assetType} with {criteria} already exists.")
    {
        AssetType = assetType;
        Criteria = criteria;
    }
}

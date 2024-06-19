using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;

public record ImageVariationCreateRequest : SharedImageRequestBaseModel
{
    /// <summary>
    ///     The image to edit. Must be a valid PNG file, less than 4MB, and square.
    /// </summary>
    public byte[] Image { get; set; }

    /// <summary>
    ///     Image file name
    /// </summary>
    public string ImageName { get; set; }
}
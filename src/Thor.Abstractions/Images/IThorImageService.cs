using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;

namespace Thor.Abstractions.Images;

public interface IThorImageService
{
    /// <summary>Creates an image given a prompt.</summary>
    /// <param name="imageCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImage(
        ImageCreateRequest imageCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates an edited or extended image given an original image and a prompt.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImageEdit(
        ImageEditCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a variation of a given image.</summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImageVariation(
        ImageVariationCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}
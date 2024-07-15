using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FileResponseModels;

/// <summary>
///     File content response
/// </summary>
/// <typeparam name="T"></typeparam>
public class FileContentResponse<T>
{
    /// <summary>
    ///     Content of your file
    /// </summary>
    public T? Content { get; set; }

    /// <summary>
    ///     return false if there is an error
    /// </summary>
    public bool Successful => Error == null;

    /// <summary>
    ///     Error
    /// </summary>
    [JsonPropertyName("error")]
    public ThorError? Error { get; set; }
}
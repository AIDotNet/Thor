using System.Buffers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

public record EmbeddingCreateResponse : ThorBaseResponse
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("data")] public List<EmbeddingResponse> Data { get; set; } = [];

    /// <summary>
    /// 类型转换，如果类型是base64,则将float[]转换为base64,如果是空或是float和原始类型一样，则不转换
    /// </summary>
    public void ConvertEmbeddingData(string? encodingFormat)
    {
        if (Data.Count == 0)
        {
            return;
        }

        switch (encodingFormat)
        {
            // 判断第一个是否是float[]，如果是则不转换
            case null or "float" when Data[0].Embedding is float[]:
                return;
            // 否则转换成float[]
            case null or "float":
            {
                foreach (var embeddingResponse in Data)
                {
                    if (embeddingResponse.Embedding is string base64)
                    {
                        embeddingResponse.Embedding = Convert.FromBase64String(base64);
                    }
                }

                return;
            }
            // 判断第一个是否是string，如果是则不转换
            case "base64" when Data[0].Embedding is string:
                return;
            // 否则转换成base64
            case "base64":
            {
                foreach (var embeddingResponse in Data)
                {
                    if (embeddingResponse.Embedding is JsonElement str)
                    {
                        if (str.ValueKind == JsonValueKind.Array)
                        {
                            var floats = str.EnumerateArray().Select(element => element.GetSingle()).ToArray();

                            embeddingResponse.Embedding = ConvertFloatArrayToBase64(floats);
                        }
                    }
                    else if (embeddingResponse.Embedding is IList<double> doubles)
                    {
                        embeddingResponse.Embedding = ConvertFloatArrayToBase64(doubles.ToArray());
                    }
                }

                break;
            }
        }
    }

    public static string ConvertFloatArrayToBase64(double[] floatArray)
    {
        // 将 float[] 转换成 byte[]
        byte[] byteArray = ArrayPool<byte>.Shared.Rent(floatArray.Length * sizeof(float));
        try
        {
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);

            // 将 byte[] 转换成 base64 字符串
            return Convert.ToBase64String(byteArray);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(byteArray);
        }
    }

    public static string ConvertFloatArrayToBase64(float[] floatArray)
    {
        // 将 float[] 转换成 byte[]
        byte[] byteArray = ArrayPool<byte>.Shared.Rent(floatArray.Length * sizeof(float));
        try
        {
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, floatArray.Length);

            // 将 byte[] 转换成 base64 字符串
            return Convert.ToBase64String(byteArray);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(byteArray);
        }
    }

    [JsonPropertyName("usage")] public ThorUsageResponse Usage { get; set; }
}

public record EmbeddingResponse
{
    [JsonPropertyName("index")] public int? Index { get; set; }

    [JsonPropertyName("embedding")] public object Embedding { get; set; }
}
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AIDotNet.SparkDesk.API
{
    public class XFSparkDeskImageGenerationAPI
    {
        private readonly XFSparkDeskAPIConfig _config;
        private HttpClient _httpClient { get; set; }

        public XFSparkDeskImageGenerationAPI(XFSparkDeskAPIConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<XFSparkDeskImageGenerationAPIResponse> GenerationAsync(XFSparkDeskImageGenerationAPIRequest request, CancellationToken cancellationToken = default)
        {
            var url = XFSparkDeskUtils.GetPostAuth(_config.HostURL, _config.ApiKey, _config.ApiSecret);

            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                header = new
                {
                    app_id = _config.AppId
                },
                parameter = new
                {
                    chat = new
                    {
                        domain = "general",
                        width = request.Width,
                        height = request.Height,
                    }
                },
                payload = new
                {
                    message = new
                    {
                        text = new List<object>()
                        {
                            new
                            {
                                role="user",
                                content=request.Content
                            }
                        }
                    }
                }
            }, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            }, cancellationToken);
            response.EnsureSuccessStatusCode();
            var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<XFSparkDeskImageGenerationAPIResponse>(rawContent) ?? throw new Exception("XFSparkDesk Image Generation Error");
        }
    }

    public class XFSparkDeskImageGenerationAPIRequest
    {
        public string Content { get; set; } = null!;

        public int Width { get; set; }

        public int Height { get; set; }
    }

    public class XFSparkDeskImageGenerationAPIResponse
    {
        [JsonPropertyName("header")]
        public XFSparkDeskImageGenerationAPIHeaderResponse? Header { get; set; }

        [JsonPropertyName("payload")]
        public XFSparkDeskImageGenerationAPIPayloadResponse? Payload { get; set; }
    }

    public class XFSparkDeskImageGenerationAPIHeaderResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [JsonPropertyName("sid")]
        public string Sid { get; set; } = null!;

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }

    public class XFSparkDeskImageGenerationAPIPayloadResponse
    {
        [JsonPropertyName("choices")]
        public XFSparkDeskImageGenerationAPIPayloadChoicesResponse Choices { get; set; } = null!;
    }

    public class XFSparkDeskImageGenerationAPIPayloadChoicesResponse
    {

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("text")]
        public List<XFSparkDeskImageGenerationAPIPayloadChoicesTextResponse> Text { get; set; } = null!;
    }

    public class XFSparkDeskImageGenerationAPIPayloadChoicesTextResponse
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}

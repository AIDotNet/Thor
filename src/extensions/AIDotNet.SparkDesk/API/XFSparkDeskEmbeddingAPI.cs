using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIDotNet.SparkDesk.API
{
    public class XFSparkDeskEmbeddingAPI
    {
        private readonly XFSparkDeskAPIConfig _config;
        private HttpClient _httpClient { get; set; }

        public XFSparkDeskEmbeddingAPI(XFSparkDeskAPIConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<XFSparkDeskEmbeddingAPIResponse> GenerationAsync(XFSparkDeskEmbeddingAPIRequest request, CancellationToken cancellationToken = default)
        {
            var url = XFSparkDeskUtils.GetPostAuth(_config.HostURL, _config.ApiKey, _config.ApiSecret);

            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                header = new
                {
                    app_id = _config.AppId,
                    status = 3
                },
                parameter = new
                {
                    emb = new
                    {
                        domain = request.Domain,
                        feature = new
                        {
                            encoding = request.ResultFeature.Encoding,
                            compress = request.ResultFeature.Compress,
                            format = request.ResultFeature.Format
                        }
                    }
                },
                payload = new
                {
                    messages = new
                    {
                        encoding = request.MessageFeature.Encoding,
                        compress = request.MessageFeature.Compress,
                        format = request.MessageFeature.Format,
                        status = 3,
                        text = request.Text
                    }
                }
            }, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            }, cancellationToken);
            response.EnsureSuccessStatusCode();
            var rawContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<XFSparkDeskEmbeddingAPIResponse>(rawContent) ?? throw new Exception("XFSparkDesk Embedding Generation Error");
        }
    }

    public class XFSparkDeskEmbeddingAPIRequest
    {
        /// <summary>
        /// 服务特性
        /// query：用户问题向量化，para：知识原文向量化
        /// </summary>
        public string Domain { get; set; } = null!;

        public XFSparkDeskEmbeddingAPIFeatureRequest MessageFeature { get; set; } = new XFSparkDeskEmbeddingAPIFeatureRequest();

        public XFSparkDeskEmbeddingAPIFeatureRequest ResultFeature { get; set; } = new XFSparkDeskEmbeddingAPIFeatureRequest();

        /// <summary>
        /// 文本数据（需base64编码）
        /// </summary>
        public string Text { get; set; } = null!;
    }

    public class XFSparkDeskEmbeddingAPIFeatureRequest
    {
        /// <summary>
        /// 文本编码	
        /// 取值：utf8, gb2312, gbk
        /// 默认：utf8
        /// </summary>
        public string Encoding { get; set; } = "utf8";

        /// <summary>
        /// 文本压缩格式
        /// 取值：raw, gzip
        /// 默认：raw
        /// </summary>
        public string Compress { get; set; } = "raw";

        /// <summary>
        /// 文本格式
        /// 取值：plain, json, xml
        /// 默认：plain
        /// </summary>
        public string Format { get; set; } = "plain";
    }

    public class XFSparkDeskEmbeddingAPIResponse
    {
        [JsonPropertyName("header")]
        public XFSparkDeskEmbeddingAPIHeaderResponse? Header { get; set; }

        [JsonPropertyName("payload")]
        public XFSparkDeskEmbeddingAPIPayloadResponse? Payload { get; set; }
    }

    public class XFSparkDeskEmbeddingAPIHeaderResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [JsonPropertyName("sid")]
        public string Sid { get; set; } = null!;
    }

    public class XFSparkDeskEmbeddingAPIPayloadResponse
    {
        [JsonPropertyName("feature")]
        public XFSparkDeskEmbeddingAPIPayloadFeatureResponse Feature { get; set; }
    }

    public class XFSparkDeskEmbeddingAPIPayloadFeatureResponse
    {
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }

        [JsonPropertyName("compress")]
        public string Compress { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}

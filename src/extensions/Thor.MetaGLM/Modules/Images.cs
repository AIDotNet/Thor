using System.Text;
using System.Text.Json;
using Thor.Abstractions;
using Thor.MetaGLM.Models.RequestModels;
using Thor.MetaGLM.Models.ResponseModels.ImageGenerationModels;

namespace Thor.MetaGLM.Modules
{
    public class Images()
    {
        private static readonly int API_TOKEN_TTL_SECONDS = 60 * 5;
        private IEnumerable<string> GenerateBase(ImageRequestBase requestBody, string apiKey)
        {
            var json = JsonSerializer.Serialize(requestBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var api_key = AuthenticationUtils.GenerateToken(apiKey, API_TOKEN_TTL_SECONDS);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://open.bigmodel.cn/api/paas/v4/images/generations"),
                Content = data,
                Headers =
                {
                    { "Authorization", api_key }
                },

            };

            var response = HttpClientFactory.GetHttpClient(request.RequestUri.AbsoluteUri).SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
            var stream = response.Content.ReadAsStreamAsync().Result;
            byte[] buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                yield return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
        }

        public ImageResponseBase Generation(ImageRequestBase requestBody, string apiKey)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in GenerateBase(requestBody,apiKey))
            {
                sb.Append(str);
            }
            return ImageResponseBase.FromJson(sb.ToString());
        }

    }
}
﻿using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.OpenAI.Chats;

public sealed class OpenAICompletionService(IHttpClientFactory httpClientFactory) : IThorCompletionsService
{
    public async Task<CompletionCreateResponse> CompletionAsync(CompletionCreateRequest createCompletionModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(OpenAIPlatformOptions.PlatformCode);

        var response = await client.PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/chat/completions",
            createCompletionModel, options.ApiKey);

        var result = await response.Content.ReadFromJsonAsync<CompletionCreateResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }
}